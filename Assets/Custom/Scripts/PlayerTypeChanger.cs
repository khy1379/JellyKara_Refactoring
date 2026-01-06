using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#region JellyStrategy class
public abstract class JellyStrategy
{
    public bool isInput;
    protected JellyKara kara;
    public JellyStrategy()
    {
        if (JellyKara.instanse != null)
            kara = JellyKara.instanse;
    }
    public abstract void UpdateMoving();
    public virtual void InputKey()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector2.zero);

            if (EventSystem.current.IsPointerOverGameObject() == false && hit.collider == null)
            {
                InputMoveStateChange();
            }
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            InputMoveStateChange();
        }
    }
    protected virtual void InputMoveStateChange()
    {
        if (isInput) return;
        isInput = true;
        kara.auSource.Play();
    }
    public virtual void InputStateReset()
    {
        isInput = false;
        if (kara != null)
        {
            kara.rb.velocity = Vector3.zero;
        }
    }
    public virtual void CollisionEnterSetting() { }
    public virtual void CollisionExitSetting() { }
}
public class SlimeStrategy : JellyStrategy
{
    public override void UpdateMoving()
    {
        if (!isInput) return;
        isInput = false;
        kara.rb.velocity = Vector2.zero;
        kara.rb.AddForce(Vector2.up * 200);
    }
}
public class BearStrategy : JellyStrategy
{
    public override void InputKey()
    {
        InputMoveStateChange();
    }
    protected override void InputMoveStateChange()
    {
        if (UnityEngine.Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector2.zero);

            if (EventSystem.current.IsPointerOverGameObject() == false && hit.collider == null)
            {
                isInput = true;
                if (kara.speed == 0) kara.speed = kara.baseSpeed;
            }
        }
        else if (UnityEngine.Input.GetKey(KeyCode.Space))
        {
            isInput = true;
            if (kara.speed == 0) kara.speed = kara.baseSpeed;
        }
        else if (UnityEngine.Input.GetMouseButtonUp(0))
        {
            isInput = false;
        }
        else if (UnityEngine.Input.GetKeyUp(KeyCode.Space))
        {
            isInput = false;
        }
    }
    public override void UpdateMoving()
    {
        if (isInput)
        {
            if (kara.maxUpMoveSpeed < kara.upMoveSpeed)
                kara.upMoveSpeed = kara.maxUpMoveSpeed;
            else
                kara.upMoveSpeed += Time.fixedDeltaTime * kara.speed;
            kara.rb.velocity = new Vector2(0, kara.upMoveSpeed);
        }
        else
        {
            if (-kara.maxUpMoveSpeed > kara.upMoveSpeed)
                kara.upMoveSpeed = -kara.maxUpMoveSpeed;
            else
                kara.upMoveSpeed -= Time.fixedDeltaTime * kara.speed;
            kara.rb.velocity = new Vector2(0, kara.upMoveSpeed);
        }
    }

    public override void CollisionEnterSetting()
    {
        kara.upMoveSpeed = 0;
        kara.speed = 0;
    }

    public override void CollisionExitSetting()
    {
        kara.speed = kara.baseSpeed;
    }
}
public class EarthStrategy : JellyStrategy
{
    bool isGravityReverse = false;
    public override void UpdateMoving()
    {
        if (!isInput) return;
        isInput = false;
        if (!isGravityReverse)
        {
            isGravityReverse = true;
            kara.rb.gravityScale = -1;
        }
        else
        {
            isGravityReverse = false;
            kara.rb.gravityScale = 1;
        }
    }
    public override void InputStateReset()
    {
        base.InputStateReset();
        isGravityReverse = false;
        if (kara != null)
        {
            kara.rb.gravityScale = 1;
        }
    }

    public override void CollisionEnterSetting()
    {
        kara.rb.velocity = Vector2.zero;
    }

    public override void CollisionExitSetting()
    {
        kara.rb.velocity = Vector2.zero;
    }
}
#endregion
public enum PlayerType
{
    Slime,
    Bear,
    Earth,

}
public interface IPlayerTypeChangeable
{
    void TypeChange(PlayerType type);
}
public class TypeChagngeObserver
{
    List<IPlayerTypeChangeable> observerList = new List<IPlayerTypeChangeable>();
    public void AddTypeChangeObserver(IPlayerTypeChangeable typeChangeClass) => observerList.Add(typeChangeClass);
    public void RemoveTypeChangeObserver(IPlayerTypeChangeable typeChangeClass) => observerList.Remove(typeChangeClass);
    public void ClearAllTypeChangeObserver() => observerList.Clear();
    public void ObserverAction(PlayerType type)
    {
        foreach (IPlayerTypeChangeable observer in observerList)
        {
            observer.TypeChange(type);
        }
    }
}
public class PlayerTypeChanger : MonoBehaviour, IKaraEventable
{
    public PlayerType pt;
    JellyStrategy curStrategy;
    JellyStrategy[] strategyArr;
    TypeChagngeObserver observer;
    public void JellyTypeInit()
    {
        observer = new TypeChagngeObserver();
        strategyArr = new JellyStrategy[3];
        strategyArr[0] = new SlimeStrategy();
        strategyArr[1] = new BearStrategy();
        strategyArr[2] = new EarthStrategy();

        int curTypeNum = PlayerPrefs.GetInt("Jelly", 0);
        curStrategy = strategyArr[curTypeNum];
        JellyTypeSet((PlayerType)curTypeNum);

        JellyKara.instanse.AddKaraEventObserver(this);
    }
    public void AddTypeChangeObserver(IPlayerTypeChangeable observerClass) => observer.AddTypeChangeObserver(observerClass);
    public void RemoveTypeChangeObserver(IPlayerTypeChangeable observerClass) => observer.RemoveTypeChangeObserver(observerClass);
    public void InputKey() => curStrategy.InputKey();
    public void JellyMoving() => curStrategy.UpdateMoving();
    public void InputStateReset() => curStrategy.InputStateReset();
    public void CollisionEnterSetting() => curStrategy.CollisionEnterSetting();
    public void CollisionExitSetting() => curStrategy.CollisionExitSetting();
    public void TypeChange(PlayerType type)
    {
        if (pt == type) return;
        JellyTypeSet(type);
    }
    public void JellyTypeSet(PlayerType type)
    {
        int typeNum = (int)type;
        PlayerPrefs.SetInt("Jelly", typeNum);
        pt = type;
        InputStateReset();
        curStrategy = strategyArr[typeNum];
        if (observer == null) Debug.Log("observer 없음");
        observer.ObserverAction(type);
    }

    public void KaraDie()
    {
        InputStateReset();
        observer.ClearAllTypeChangeObserver();
    }

}
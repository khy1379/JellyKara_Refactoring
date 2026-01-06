using UnityEngine;
using UnityEngine.EventSystems;

public abstract class JellyStrategy
{
    protected JellyKara kara;
    public bool isInput;
    public JellyStrategy()
    {
        if(PlayerTypeChanger.kara != null)
            kara = PlayerTypeChanger.kara;
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
}
public enum PlayerType
{
    Slime,
    Bear,
    Earth,

}
public class PlayerTypeChanger : MonoBehaviour
{
    public static PlayerType pt;
    public static JellyKara kara;
    static JellyStrategy curStrategy;
    static JellyStrategy[] strategyArr;
    public static bool IsInput => curStrategy.isInput;
    private void Awake()
    {
        JellyInit();
    }
    void JellyInit()
    {
        kara = GetComponent<JellyKara>();
        strategyArr = new JellyStrategy[3];
        strategyArr[0] = new SlimeStrategy();
        strategyArr[1] = new BearStrategy();
        strategyArr[2] = new EarthStrategy();

        switch (PlayerPrefs.GetInt("Jelly", 0))
        {
            case 1:
                curStrategy = strategyArr[1];
                TypeSetBear();
                break;
            case 2:
                curStrategy = strategyArr[2];
                TypeSetEarth();
                break;
            case 0:
            default:
                curStrategy = strategyArr[0];
                TypeSetSlime();
                break;
        }
    }
    public static void InputKey()
    {
        curStrategy.InputKey();
    }
    public static void JellyMoving()
    {
        curStrategy.UpdateMoving();
    }
    public static void InputStateReset()
    {
        curStrategy.InputStateReset();
    }
    public static void TypeChange(PlayerType type)
    {
        switch (type)
        {
            case PlayerType.Slime:
                TypeSetSlime();
                break;
            case PlayerType.Bear:
                TypeSetBear();
                break;
            case PlayerType.Earth:
                TypeSetEarth();
                break;
        }
    }
    public static void TypeSetSlime()
    {
        PlayerPrefs.SetInt("Jelly", 0);
        pt = PlayerType.Slime;
        curStrategy = strategyArr[0];
    }
    public static void TypeSetBear()
    {
        PlayerPrefs.SetInt("Jelly", 1);
        pt = PlayerType.Bear;
        curStrategy.InputStateReset();
        curStrategy = strategyArr[1];
    }
    public static void TypeSetEarth()
    {
        PlayerPrefs.SetInt("Jelly", 2);
        pt = PlayerType.Earth;
        curStrategy.InputStateReset();
        curStrategy = strategyArr[2];
    }
}

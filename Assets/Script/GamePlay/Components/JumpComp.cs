[CompRegister(typeof(JumpComp))]
public class JumpComp : Comp
{
    public float baseY;

    public float curY;

    public float duration;

    public float scale;

    public float speed;

    public float gravity;

    public bool isJump;

    public bool startJump;

    public bool onLand;

    //public float lower;

    public override void Reset()
    {
        //lower = 1.0f;
        baseY = 0;
        curY = 0;
        duration = 0;
        scale = 0;
        speed = 0;
        gravity = 0;
        isJump = false;
        startJump = false;
        onLand = true;
    }
}


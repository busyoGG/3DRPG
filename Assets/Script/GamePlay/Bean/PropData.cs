public class PropData
{
    public int hp;

    public int mp;

    //public int sheild;

    public int curHp;

    public int curMp;

    public int curSheild;

    //public int group;

    public PropData(int hp,int mp)
    {
        this.hp = hp;
        this.mp = mp;
        //this.sheild = sheild;
        //this.group = group;
    }
    public PropData() {
        this.hp = 0;
        this.mp = 0;
        //this.sheild = 0;
        //this.group = -1;
    }
}

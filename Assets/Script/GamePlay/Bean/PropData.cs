public class PropData
{
    public int hp;

    public int mp;

    //public int sheild;

    public int curHp;

    public int curMp;

    public int curSheild;

    //¹¥»÷Á¦
    public int attack;

    public PropData(int hp,int mp,int attack)
    {
        this.hp = hp;
        this.mp = mp;
        this.attack = attack;
        //this.sheild = sheild;
        //this.group = group;
    }
    public PropData() {
        this.hp = 0;
        this.mp = 0;
        this.attack = 0;
        //this.sheild = 0;
        //this.group = -1;
    }
}

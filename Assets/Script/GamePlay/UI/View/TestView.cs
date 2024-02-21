using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestView : BaseView
{
    [UIDataBind("TextField", "")]
    public UIProp test { get; set; }

    protected override void InitAction()
    {
        test.Set(1);
        ConsoleUtils.Log("test====>", test);
    }
}

# Dynamic modify parameter frame for unity game.

2017-02-21

工程目标：
根据之前做的一个控制台的输入，想根据控制台的输入，来控制和修改游戏中的某些参数。

目前大部分简单游戏修改参数有数据库，XML，json等各式各样，现在做的是一个可以在游戏过程中，实时修改任意变量的一个东西。
用法很简单，tab按键来打开控制台，输入需要修改的参数就可以，看到参数实时被修改。
也可以把它理解成简单的修改器，用于调节参数和可能的命令修改游戏中的参数。

当然，也需要稍微修改一个项目中原有的代码。这个功能已经使用代码一键实现了，并且使用unity中不加入编译的方式，在代码文件名字前加“.”的方式保留了原来的代码。



目前的还需要做的事情：

1.命令行需要丰富，现在控制台命名只有两个，一个是清屏clear，一个是退出控制台exit。

2.控制台命令行输入提示，根据需要提示和回滚之前的输入信息。


关于工程的一些说明：

http://blog.csdn.net/cartzhang/article/details/56292977

更多控制台的输入项目工程地址和说明：

https://github.com/cartzhang/UnityConsoleWindow

http://blog.csdn.net/cartzhang/article/details/49818953

http://blog.csdn.net/cartzhang/article/details/49884507


Thanks to you all.

@cartzhang



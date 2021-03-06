## Thinking

* 想做一个替代FlowCanvas的蓝图方案
* FlowCanvas初始化太慢，对象创建的太多
* xNode的数据构成比较纯粹，有戏
* 就是运行时要自己写了

### 代码生成 or 节点运行
* 代码生成的性能更好，许多数据直接存于栈上，免去函数套壳
* 但是代码生成会使之不再是个数据，这会带来两个问题
    * 由于不是数据，资源数据的初始化会待到对象初始化时
    * 由于不是数据，不便配置

### 蓝图
* 蓝图应是资源，这代表着两点
    * 蓝图被加载时也会顺带加载所引用的其他资源
    * 不同的对象实例引用相同的蓝图资源
* 蓝图的资源构成
    * 节点
    * 连线关系
    * 节点上的资源
* 使用实例化的虚拟机驱动蓝图运行

### 函数
* 蓝图的执行单元为函数，它表现为一连串的节点，在节点首位通过「函数节点」命名
* 函数通过虚拟机通过名字调用
* 函数没有参数和返回值
* 可以在蓝图里调用函数

### 节点
* 节点代表一系列的输入/输出数据
* 为了表示执行顺序，节点的第一位输入与输出数据为`In, Out`，节点之间通过连接InOut进行串联
* 当然也会有不参与流程、只是为某些节点提供数据的节点，对于它们不连InOut即可
* 也会有着特殊的、含有多重InOut的节点，如条件分支
* 通常节点只可能有一个返回值，名为Ret

### 虚拟机
* 运行蓝图的实例化对象
* 通过提供蓝图资源，调用蓝图函数运行
* 虚拟机的存储数据主要是InOut上有被引用到的输出数据
    * 以免后续用到重复运算
* 以及变量

### 变量
* 存储在虚拟机的数据，通过`Dictionary<string, object>`保存
* 在蓝图可通过节点设置与获取变量
    * Plan A: 变量节点可选择不同类型进行转义？
    * Plan B: 利用代码生成转义节点？
* 同理，可以在虚拟机设置与获取变量
* 扩展：设计专门的面板，可配置变量到蓝图作为数据存储，初始化时赋值给虚拟机

## 运行
* 每个节点有着自己的运行函数`Run()`
* Run执行完毕后，调用Out插槽引用的节点的Run，以此类推
* Run有着返回值

## 参数值的获取
* 需要有一套很通用的获取值的接口
* 可接受一定的代码生成辅助
* 要保证性能
* 当该属性没有连接节点时，取自身值，否则取连接节点的返回值
* 连接关系描述采用的是变量文本，无法直接与变量值对应
    * 可采用一定的手写、代码生成对应
* 运行时再去获取连接节点会很磨叽
    * 考虑在资源加载时设置？
* 缺乏健壮的取值方式
    * 看看怎么封装一个
* 对于运行于主干上有返回值的节点需要做缓存
    * 避免重复运算

## 异步
* 异步具有传染性，得整个环境都是async
* 异步会使得函数调用嵌套加深，稍微影响性能
* 异步会创建Task返回值，产生GC
* 但其实如果只是单次调用，开销还算能接受
* 考虑同步异步按两套实现？
* 调用时标明是否使用异步
* 节点的`Run`与`GetValue`设计两套
* 使用变量判断标识异步节点
* `RunFunc`拥有等待/非等待的两套函数
* 蓝图初始化后会识别所有Func是否含有异步节点
* 根据Func是否含有异步节点决定调用方案
* 在蓝图内调用Func可选是否等待

## 中断
* 必须提供中止当前事务的机制与节点
* 类似于函数的return
* 在`RunFunc`时可传入专用id，可通过id调用`ExitFunc`中断执行，每个节点执行时将判断当前id是否已结束
* 在节点的`Run`函数将会被传入id，以便操作
* id不填写时将会根据func的哈希值生成

## 分支
* 分支的本质是根据一个条件值，决定两条路线的二选一，最后继续走Out

## 界面
* 默认的界面无法满足我们的需求，主要分为节点、预览、菜单三项
* 节点
    * 需要自定义节点名字，支持中文
        * 通过继承NodeEdtior，修改OnHeaderGUI
* 预览
    * 添加名称和注释
    * 直接使用Odin加持
* 菜单
    * 改为分类的、可搜索的节点列表库，中英文都显示
    * 如：调试输出(Log)
    * 使用Odin开发节点列表库，继承GraphEditor，修改呼出

### 节点生成
* 部分特殊节点手动编写
* 多数节点通过属性`NodeAttribute(title, note)`标记函数自动生成代码
* 只支持静态函数生成节点（懒）
* 编写专门的编辑器代码，通过在菜单进行代码生成
* 以函数为单位生成节点类文件，根据命名空间与类生成文件路径
* 节点内容除了输入输出数据，还包括功能函数（供虚拟机调用）

### object作为参数
* object不支持序列化，所以无法存储在节点
* 但实际上不需要真的存储，因为object注定是接受其他节点的数据
* 使用Obj对象包装
* 设置变量节点需判断对象是否Obj，若是则取出其值
* 获取变量节点返回值需使用Obj包装出去
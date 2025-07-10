

# Tick Tools
基于PlayerLoopSystem进行的轮询流程开发的小工具库
- 轻量单线程async插件
- 移动端手势识别插件


## 异步工具说明
说明：所有的等待都是在主线程中执行，不涉及线程切换，可循环嵌套使用
- await new TwistAsyncWaitCondition(Func<bool>  condition) 等待一个条件为真后继续执行
- await new TwistAsyncWaitFrame(int waitFrame) 等待几帧后继续执行
- await new TwistAsyncWaitSecond(float second) 等待几秒后继续执行
- await new TwistBreak() 跳出当前的异步等待
- await new (PlayerLoopTiming playerLoopTiming,Action onStart = null,Action onCompleted = null) 向特定的时间节点添加钩子

## 手势识别说明

说明：所有的交互事件都是在LastEarlyUpdate中进行触发

使用方法：场景中挂在脚本 "FingerEventHandle" 组件挂在场景中任意对象身上【每个场景中有且只有一个】，FingerEventHandle是MonoBehaviour单例，使用FingerEventHandle.Current获取。

**FingerEventHandle**
 - float m_pressInterval：点击触发的最大事件间隔
 - SingleFingerDetectionState singleFingerDetectionState：检测模式，2D、3D或者都检测
 - LayerMask m_checkLayerMask：检测的对象层级
 - bool m_isFIngerDonw：是否存在按下的鼠标或手指
 - Action<FingerOperationData> OnFingerDown：当有手指或鼠标按下的时候触发
 - Action<FingerOperationData> OnFingerUp：当有手指或鼠标抬起的时候触发
 - Action<FingerOperationData> OnDoublePress：当有手指双击的时候触发
 - Action<FingerOperationData> OnPress：当有手指点击的时候触发
 - Action<FingerOperationData> OnFingerHold：当有手指按着动或者不动的时候触发

**FingerOperationData**

 - int fingerId：当前操作的ID
 - float downTime：当前ID按下去的时间
 - float prePressTime：当前ID上一次单击按下的时间
 - bool IsFingerDown：当前ID是否按下
 - Vector2 FingerScreenDownPos：当前ID手指在屏幕上按下的位置
 - Vector2 FingerScreenUpPos：当前ID手指在屏幕上抬起的位置
 - Vector2 FingerScreenCurrenPos：当前ID手指，现在在屏幕中的位置
 - Vector2 FingerScreenOffset：当前ID手指上一帧的位移
 - GameObject CheckCurrentDown：手指按下去的那一刻检测到的对象
 - GameObject CheckCurrentHold：当前手指下面被检测到的对象
 - GameObject DownUI：手指按下去的那一刻检测到的UI【UGUI】

**DragGestureEvent**
挂载在需要拖拽的对象身上，并且添加好FingerEventHandle检测模式对应的碰撞体，为Action订阅事件

 - bool IsRestrictionSingleFinger：是否限制单指，勾选后只有ID为0的手指可以进行交互
 - DragCheckMode _DragCheckMode：拖拽方式，水平、垂直、总偏移距离
 - float TriggerDistance：拖拽触发的距离
 - Action<FingerOperationData> OnDragBeginningEvent：开始拖拽的时候执行
 - Action<FingerOperationData> OnDraggingEvent：拖拽中执行
 - Action<FingerOperationData> OnDragEndEvent：拖拽结束后执行

**PressGestureEvent**
挂载在需要监听点击事件的对象上，设置好碰撞体，为Action订阅事件

 - bool IsRestrictionSingleFinger：是都显示单指，勾选后只有ID为0的手指才可以进行交互
 - Action<FingerOperationData> OnPressEvent：当对象被点击的时候执行

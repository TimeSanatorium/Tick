
# Tick Tools
基于PlayerLoopSystem进行的轮询流程开发的小工具库
- 轻量级异步小工具
- 移动端单指手势识别小工具


## 异步工具说明
说明：所有的等待都是在PlayerLoopSystem中执行，不涉及线程切换
- await new TwistAsyncWaitCondition(Func<bool>  condition) 等待一个条件为真后继续执行
- await new TwistAsyncWaitFrame(int waitFrame) 等待几帧后继续执行
- await new TwistAsyncWaitSecond(float second) 等待几秒后继续执行
- await new TwistBreak() 跳出当前的异步等待
- await new (PlayerLoopTiming playerLoopTiming,Action onStart = null,Action onCompleted = null) 向特定的时间节点添加钩子

## 手势识别说明

说明：所有的事件都是在LastEarlyUpdate中进行识别的，目前能自动适配PC和移动端单指操作，移动端永远更新fingerid最小的那个

使用方法：将SingleFingerEventHandle组件挂在场景中任意对象身上【只要挂一个】

*信息说明*
 - PressInterval：点击事件识别按下和抬起最大间隔时间
 - FingerScreenDownPos：手指按下的屏幕位置
 - FingerScreenUpPos：手指抬起的屏幕位置
 - FingerScreenCurrenPos：当前手指所在的位置
 - FingerScreenOffset：当前帧手指在屏幕上的位移
 - IsFIngerDonw：是否有手指按下
 
 *方法说明*
 - OnFingerDown：委托，当有第一根手指按下的时候调用
 - OnFingerUp：委托，当最后一根手指抬起的时候调用
 - OnFingerHode：委托，当有手指停在屏幕上的时候调用
 - OnPress：委托，当手指点击的时候调用
 - OnDoublePress：委托，当手指双击的时候调用


# 验证码识别演示程序


 - 打开 bin 下 `\RECAPTCHA\RECAPTCHA\bin\Debug` 的 `CaptchaRecogition.exe`
 
 - 选择要处理的验证码图片位置，也可以网络获取下载：比如选择bin下：`\RECAPTCHA\RECAPTCHA\bin\Debug\captcha\jinshannetwork`
 
 - 选好验证码图文位置后，依次点击 `灰度 去背景 去噪点 二值化 图片分割`，查看处理前后的效果
 
 - 最上方可以设置灰度方式，噪点阈值；字符宽度，字符数可以不强制指定，不指定会根据边缘检测算法自动切割，如果字符粘连严重，可以适当的指定下这两个值。
 
字模制作：
 - 选择好字模位置，就是一个txt文档。bin下`RECAPTCHA\RECAPTCHA\bin\Debug\captcha\jinshannetwork\zimo.txt`开始识别，如果识别的不正确，点击`识别错误-->修改`按钮，修改错误的字符，然后点击 `字模学习入库`按钮
 
详细处理过程可参考文章：
## http://www.cnblogs.com/cnduan/p/5154419.html
 
**为了方便大家实验，实验中用到的验证码图片，字模等资源都放在项目的`bin`下** 
 
# 发票编号识别案例：

 - 这个演示过程需要调用计算机摄像头，测试用例是识别上海地铁纸质发票。
 - 具体过程是 `AForge`捕捉摄像头中发票图片，确定发票标号位置，调用验证码处理逻辑，切割发票编号，匹配字模识别并展示在图像上。
 - 字模是通过上面的程序手动训练得到的，放到了bin下 `\发票识别\发票编号识别\bin\Debug\zimo.txt`。
 - 具体效果可以参考：
 
## http://v.youku.com/v_show/id_XMTI1MzUxNDY3Ng==.html
 
 

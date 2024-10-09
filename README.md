# GPACapture2OBJ

## 1.说明
因为Intel GPA截帧获取的模型丢失uv，法线也有异常，所以弄出这个工具来还原。  
看网上教程可以用houdini还原，但我不太想碰houdini，看了下GPA导出的法线/uv.csv文件大概猜出来是做什么的，就动手做工具了。  
本来想用PowerShell .ps1来写，但是频繁查语法也花时间，最后还是用了熟悉的c#来写。  

## 2.使用方式
1.从GPA导出要截取的模型.obj文件；  
2.导出对应模型的法线.csv和uv.csv文件；  
3.将以上文件放到同一个文件夹内，重命名以上3个文件，示例：  
>./test.obj (模型文件)  
>./test_normals.csv (法线csv文件)  
>./test_uvs.csv (uv csv文件)  
  
4.PowerShell运行GPACapture2OBJ.exe，输入参数写要处理的文件夹（不写则处理GPACapture2OBJ.exe所在文件夹下的文件），命令示例：  
`.\GPACapture2OBJ.exe D:/WORK/`  
5.执行成功会看到文件夹下生成test_handled.obj文件，就是已经处理好的模型文件。  

注:文件夹下可以放置多组文件，工具会全部处理（***_handled.obj命名的文件会被忽略）  

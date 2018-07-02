# vsix-bug-quickinfosession
Simple VSIX project for VS2015/2017 that sports a bug.

Bug behaviour when using QuickInfo Tooltips:

![tooltip-jumps](https://github.com/HJLebbink/vsix-bug-quickinfosession/blob/master/Images/window_moves.gif?raw=true "Tooltip Moves")

See Issue [here](https://github.com/Microsoft/vs-editor-api/issues/11).

This minimal extension creates a filetype with extension .xyz. If you run (debug) this extension and open a (text) file with extension .xyz, hovering the mouse over any word will show a tooltips with the described bug. I only tested this extension with VS2017 15.7.4. For convenience a dedicated output window will be opened with some logging from this extension.

I you have any questions, don't hesitate to ask me.


## this project has also been used to describe other bugs:

1. See SO [here](https://stackoverflow.com/questions/46793168/quickinfosession-is-dismissed-prematurely-when-using-usercontrols-in-quickinfoco?noredirect=1#comment88979250_46793168)
2. MSDN forum [here](https://social.msdn.microsoft.com/Forums/vstudio/en-US/7084cf4c-639a-4aaf-9027-b71cdad27acd/tooltip-with-quickinfosession-with-a-usercontrol-instead-of-a-string?forum=vsx#7084cf4c-639a-4aaf-9027-b71cdad27acd)
3. MSDN forum [here](https://social.msdn.microsoft.com/Forums/vstudio/en-US/629225b5-2a53-4313-8526-6644013ab120/quickinfosession-is-dismissed-prematurely-when-using-usercontrols-in-quickinfocontent?forum=vsx#629225b5-2a53-4313-8526-6644013ab120)


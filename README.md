XDCCBatchDownloader
===================

____________________________________________________________________________________________________
What is it?:
-------------------

Well, its propably already clear what it does if you read the name of the program, but yeah, this console program
can download every episode available from an anime that you specify, as long its available on the bot. It is able to resume if you stop midway to, i mean its able to resume per episode, so if you stopped while it was downloading one, it will not start downloading that episode again, it will continue to the next episode, I will explain down below what to do about it. It is also able to stop and start the irc client when a file is finishd downloading, which I am actually proud of :).

____________________________________________________________________________________________________
Why?:
-------------------

Ehm, the reason I created this is because I am lazy, I do not want to manually download every episode by hand, especially on my phone its a pain in the ass, it takes a hole freaking 5 minutes to start downloading an episode using TurboIRC as client, they do not name the files, so its hard to tell which I downloaded :(, so I came up with this. There are propably more of these kind of xdcc/dcc downloaders but I wanted to create my own. I just love XDCC for its speed and that you can play the file immediatly after starting the download.


____________________________________________________________________________________________________
How to intstall it and how to use it:
-------------------
THIS IS LINUX ONLY FOR NOW!!!
Yep, I made this mainly for the raspberry pi.
So, before this works you will have to set things up correctly:

1. sudo apt-get install mono-complete.

2. sudo apt-get install irssi.

3. run irssi by typing in terminal: irssi.

4. close it by typing /quit.

5. open /home/(user) and make sure that you can see hidden folders. (Use WinSCP if you want to control your pi remotely)

6. locate .irssi folder and open it.

7. put XDCCBatchDownloader.exe, config, logfile.txt and settings.ini in it.

8. change nickname etc in config file to your liking, do not use the one provided with it, since it might be in use.

9. open settings.ini and change the search option into the name of the anime you want, including release/subgroup!

9. 1. change bot option to the bot you would like to download from, you can get a list from intel.haruhichan.com

9. 2. change channel option into the channel where you can access that bot, that is very important otherwise it wont work!

9. 3. only change resolution if the sub/releasegroup specifys it in his file name(like [horriblessubs] blabla [720p].mkv), 
   leave it blank when they do not specify it!

10. start XDCCBatchDownloader.exe by typing cd /home/(user)/.irssi, or if you are already in home: cd .irssi 
and then (sudo) mono XDCCBatchDownloader.exe

11. now it works, your files will be dlld to you home/(user) folder, option to change this will be added in the future

YOUR DONE!

____________________________________________________________________________________________________
Future functions:
-------------------
Multiple anime support

Windows functionality

Change Download Location 

Option to specify from which to which episode you want to download

____________________________________________________________________________________________________
How it works:
------------------
It retrieves the html code from intel.haruhichan.com/?s="search", retrieves all episodes from the anime+subgroup 
from the bot you specified, puts them in the right order(they are normally scrambled), together with the retrieved episodes there are packnumbers, * per episode it will change the config file of irssi to include an autolaunch command like /join [channelname];/msg [botname] xdcc send #[packnum], and when the config is changed, it will launch the irc client (irssi), while irssi is downloading the file, a loop is checking if the file that irssi is downloading changes filesizes every 10 seconds, if not, I assumed the file would be finishd downloading, when that happens it will close the irc client and continue to the next episode, repeating everything starting from *. If somehow the file is not retrieved the program will stop. The problem is most likely you were banned from the irc channel or something like that.

____________________________________________________________________________________________________
FAQ:
------------------
(I do not have any FAQ since noone uses it now, but their are some things you might crash into)

1. I stopped/The program crashed/stopped in the middle of downloading an episode, how can I still get that episode?
Go to your .irssi folder and locate logfile.txt, open it and delete the last line, then start XDCCBatchDownloader     again. It should redownload that episode.

2. The program tells me that it could not open settings.ini, logfile.txt or config, what should I do?
First make sure you took every file from the folder (necessary files) on this github page and put them in the same
folder where XDCCBatchDownloader is. That should propably be .irssi .

3. My programmed stopped responding, CTRL-C does not exit the program. 
That happens most likely because it finishd downloading or that something went wrong with the irc client, anyway
the only thing that you could propably do is restart the terminal/ssh session, if you are directly on the rpi, you 
propably should restart the RPi, but I didnt test it out, so it might not crash directly on the RPi. I know that      this shouldnt happen, but I do not really know why it does that, yet.

4. Where is the executable file?
I will put in the folder (necessary files) here on github, but you can also find it in bin/debug

5. I have a suggestion/ idea, where can I tell you? 
I will make a forum topic on haruhichan.com, there you can give me suggestions or anything else related to my         program. The link is: http://haruhichan.com/forum/ ... <- did not make it yet

____________________________________________________________________________________________________
Future Vision:
------------------
A popcorn time ish anime watcher / downloader. And it will be more reliable since it does not need the use of torrents, xdcc are generaly faster then torrents, and nomatter how old the file on the server is, it will have equal speeds to new one(but it has to be on the server). No more waiting until it is seeded, it will be press and play :). Now, this might never become true, since that requires a shit ton of work and I am on my own, with an programming language wich i just started at, and my study just starting, which requires a lot of time. So for now it will be a simple automated downloader :)

____________________________________________________________________________________________________
DISCLAIMER(or atleast what i think its called):
------------------

I DO NOT OWN IRSSI NOR DO I OWN ANYTHING HERE BESIDES ALL THE SCRIPTS / PROGRAMS POSTED HERE!!!
If you want to change the code, go ahead, its free for all, I just would like some credit if you think of changing/using the code for your own good :), not in money, but just like a comment in your code like: // this is created by rareamv. BUt I leave that up to you, you can do with it whatever you want.

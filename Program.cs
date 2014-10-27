using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

// This script assumes that every episode number starts with 01, also asumes its always between the first ] and [ lets say [subgroup]aname- epnum[bd/size etc][something]
// so for it to get the ep num it just strips everything before and after ][ away to get the name, and then the episode number. Also asumses that there is no 0 in the
// anime title, if so, it wont work... <- this script assumes a lot, and I cannot be more accurate for now!
namespace XDCCBatchDownloader
{
	class MainClass
	{


		public static void Main (string[] args)
		{

			getSettings ("settings.ini");
			scanIntel (bot, search, reso);
			Console.ReadLine();
		}


		public static int starter = 0;

		public static string readIntel(string  url){


			var webClient = new WebClient();

			string intel = webClient.DownloadString (url);


			return intel;
		}

		public static StringBuilder animeepsb  = new StringBuilder();

		public static string[] explode(string separator, string source) {
			return source.Split(new string[] { separator }, StringSplitOptions.None);
		}

		public static void scanIntel(string botname, string search, string size){
			string urlpart = search.Replace (" ", "%20");
			string data = readIntel ("http://intel.haruhichan.com/?s=" + urlpart);
			string scanStart_1 = "<tbody>";
			string scanEnd_1 = "</tbody>";


			try{
				int scanStart1 = data.IndexOf (scanStart_1);
				string partiald = data.Substring (scanStart1);
				int scanEnd1 = partiald.IndexOf (scanEnd_1);
				partiald = partiald.Substring (0, scanEnd1);

				string scanStart_2 = "<tr class=";
				string scanEnd_2 = "</tr>";
			


				Console.WriteLine ("start: " + scanStart1 + " end: " + scanEnd1);


			
				int x = 0;
				int scanStart2 = 0;
				string partiald2  = String.Empty;
				string fullstring = String.Empty;
				string oldname = String.Empty;
				string botpack = String.Empty;
				while(true){
					try{
						if(x == 0){
							scanStart2 = partiald.IndexOf(scanStart_2);
						} else {
							scanStart2 = partiald.IndexOf(scanStart_2);
						}
						partiald2 = partiald.Substring(scanStart2 + 10);
						partiald = partiald2;
						int scanEnd2 = partiald2.IndexOf(scanEnd_2);
						partiald2 = partiald2.Substring(0, scanEnd2);
					    fullstring = partiald2;
					} catch {
						Console.WriteLine("Dont worry, its fine, but it crashed on the anime name search part, so its fine when the total eps is righ:P");
						break;
					}






					if(partiald2.Contains("p2")){
						try{
							partiald2 = partiald2.Replace("p2 noselect\"><td>", "");
							partiald2 = partiald2.Replace("</td><td>", "");
							partiald2 = partiald2.Replace("</td>", "");
							int removetill = partiald2.IndexOf("[");
							partiald2 = partiald2.Substring(removetill);
						} catch {
							Console.WriteLine("Ignore this, its alright, but it crashed in p2 cutaway code part");
							break;
						}
					} else {
						try{
							partiald2 = partiald2.Replace("p1 noselect\"><td>", "");
							partiald2 = partiald2.Replace("</td><td>", "");
							partiald2 = partiald2.Replace("</td>", "");
							int removetill = partiald2.IndexOf("[");
							partiald2 = partiald2.Substring(removetill);
						} catch {
							Console.WriteLine("Ignore this, its alright, but it crashed in p1 cutaway code part");
							break;
						}
					}

					x++;

					try{
						string[] arbotsfinder = explode("</td><td>", fullstring);
						botpack = arbotsfinder[1]; 
						Console.WriteLine("botpack = " + botpack);
						Console.WriteLine("fullstring: " + fullstring);
					} catch {
						Console.WriteLine("Something Went wrong while finding botpack!");
						Console.WriteLine("fullstring: " + fullstring);
						break;
					}
					try{
						if(size != ""){
							try{
								if(fullstring.Contains(botname) && partiald2.Contains(size)){
									if(partiald2 != oldname){
										Console.WriteLine("bot found!");
										Console.WriteLine ("anime string: " + x + " : " + partiald2 + " : " + size);
										Console.WriteLine("fullstring = " + fullstring);
										animeepsb.Append(partiald2 + "^" + botpack + "$");
									} else {
										Console.WriteLine("double detected, all episodes listed");
										break;
									}
									oldname = partiald2;
								}
							} catch {
								Console.WriteLine("could not add to array using size");
							}
						} else {

							bool sizeavailable = false;
							if (partiald2.Contains("720p")){
								Console.WriteLine("720p is available, please set this in the size option");
								sizeavailable = true;
							}
							if (partiald2.Contains("1080p")){
								Console.WriteLine("1080p is available, please set this in the size option");
								sizeavailable = true;
							}
							if(partiald2.Contains("480p")){
								Console.WriteLine("480p is available, please set this in the size option");
								sizeavailable = true;
							} 

							if(fullstring.Contains(botname) && sizeavailable == false){
								if(partiald2 != oldname){
									Console.WriteLine("bot found!");
									Console.WriteLine ("anime string: " + x + " : " + partiald2);
									Console.WriteLine("fullstring = " + fullstring);
									animeepsb.Append(partiald2 + "^" + botpack + "$");

								} else {
									Console.WriteLine("double detected or size not specified, all episodes listed");
									break;
								}

								oldname = partiald2;

							}


						}
					} catch {
						Console.WriteLine("stuff went wrong while finding fitting episode to all demands, or appending to string");
					}
				}

				Console.WriteLine("____________________________________EPS VOLOGRDE_______________________");
				string animeepisodes = animeepsb.ToString();
				string[] animeepsar = animeepisodes.Split('$');
				int animeepsarlength = animeepsar.Length;
				Console.WriteLine("array length = " + animeepsarlength);
				string part_of_cont = String.Empty;
				int posofepnum = 0;

				try{
					int q = 0;
					while(true){
						try{
							Console.WriteLine("anime ep: " + animeepsar[q]);
							int posofstartaname = animeepsar[q].IndexOf("]");
							part_of_cont = animeepsar[q].Substring(posofstartaname);
							int posofendaname = part_of_cont.IndexOf("[");
							part_of_cont = part_of_cont.Substring(0, posofendaname);
							posofepnum = part_of_cont.IndexOf("0");

							if(posofepnum > 1 && !part_of_cont.Contains("10")){
								Console.WriteLine("match found");
								break;
							}
						} catch{
							Console.WriteLine("end of array or could not find part of epnum");
							break;
						}
						q++;
					}

					Console.WriteLine("Putting stuff in the right order using: " + part_of_cont);
					Console.WriteLine("posofepnum = " + posofepnum);

					try{
						var aStringBuilder1 = new StringBuilder(part_of_cont);
						aStringBuilder1.Remove(posofepnum, 2);
						aStringBuilder1.Insert(posofepnum,  "0" + 5);
						string preview = aStringBuilder1.ToString();
						Console.WriteLine("possible outcome for search: " + preview);

					} catch {
						Console.WriteLine("could find name of ep but cannot change ep num ??");
					}
				} catch {
					Console.WriteLine("stuff aint finding shit, so no episode num will change");
				}






				int y = 0;


				while (y != animeepsarlength){
					y++;
					int z = 0;
					string cont = String.Empty;
					string num = y.ToString();
					if(y < 10){

						var aStringBuilder = new StringBuilder(part_of_cont);
						aStringBuilder.Remove(posofepnum, 2);
						aStringBuilder.Insert(posofepnum,  "0" + num);
						cont = aStringBuilder.ToString();
						//Console.WriteLine("below10: " + cont);
					}else{

						var aStringBuilder = new StringBuilder(part_of_cont);
						aStringBuilder.Remove(posofepnum, 2);
						aStringBuilder.Insert(posofepnum, num);
						cont = aStringBuilder.ToString();
						//Console.WriteLine("above9: " + cont);

					}
					//Console.WriteLine("test: " + cont);
					while(z != animeepsarlength){

						try{
							if(animeepsar[z].Contains(cont)){
								Console.WriteLine("anime eps desc: " + animeepsar[z]);
								string[] botnumberar = animeepsar[z].Split('^');
								string botnumber = botnumberar[1];
								string animeepfile = botnumberar[0];
								try{
									getConfig("config", botnumber, animeepfile);
								} catch {
									Console.WriteLine("botnumb = " + botnumber);
									Console.WriteLine("animeepfile = " + animeepfile);
									Console.WriteLine("Could not launch getConfig!");
								}
							}
						} catch {
							Console.WriteLine("stuf aint right in launching part of the code!");
							break;
						}
						z++;
					}



				}
				Console.WriteLine("array lenth: " + animeepsarlength);
			} catch {
				Console.WriteLine ("cannot find pos or cannot cut string");
			}


		

		}

		public static string search = String.Empty;
		public static string bot = String.Empty;
		public static string channel = String.Empty;
		public static string reso = String.Empty;
		public static int sleep = 0;

		public static void getSettings(string filename){

			try{
				var settingsopen = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				var content = new StreamReader(settingsopen, Encoding.Default);

				string setcontent = content.ReadToEnd();
				setcontent = setcontent.Replace(Environment.NewLine, "");
				string[] settings = setcontent.Split ('^');
				string[] search_param_ar = settings [0].Split ('=');
				string[] bot_param_ar = settings [1].Split ('@');
				string[] bot_name_ar = bot_param_ar[0].Split ('=');
				string[] channel_ar = bot_param_ar[1].Split('=');
				string[] resolution_param_ar = settings [2].Split ('=');
				string[] dlspeed_param_ar = settings[3].Split('=');

				string dlspeed = dlspeed_param_ar[1];
				int dl_speed = 0;

				try{
					dl_speed = Convert.ToInt32(dlspeed);
					sleep = 1000/dl_speed*1000;

				} catch{
					Console.WriteLine("something went wrong while retrieveing dl time");
				}
				search = search_param_ar [1];
				bot = bot_name_ar [1].Replace(" ", "");
				channel = channel_ar [1];
				reso = resolution_param_ar [1].Replace(" ", "");


				Console.WriteLine("total sleep time in ms: " + sleep);
				Console.WriteLine ("search= " + search);
				Console.WriteLine ("bot= " + bot);
				Console.WriteLine ("channel = " + channel);
				Console.WriteLine ("resolution = " + reso);
			} catch {
				Console.WriteLine ("Could not open settings.ini or something is not right in settings.ini!");
			}
				
		}

		public static void getConfig(string filesource, string packnums, string epname){

			//StreamReader file = File.OpenText(filesource);
			try{
				var configopen = new FileStream(filesource, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				var content = new StreamReader(configopen, Encoding.Default);
				int packnum = Convert.ToInt32 (packnums);
				string concontent = content.ReadToEnd();
				searchConfig(concontent, packnum, bot, epname);
			} catch {
				Console.WriteLine ("Could not open config file!");
			}


		}

		public static void searchConfig(string content, int packnumb, string bot, string epname){



			string searchforstartcmd = "autosendcmd";
			int posofstartcmd = content.IndexOf (searchforstartcmd);
			string ascmd_unref = content.Substring (posofstartcmd);
			string searchforendcmd = "\";";
			int posofendcmd = ascmd_unref.IndexOf (searchforendcmd);
			string ascmd = ascmd_unref.Substring (0, posofendcmd);
			Console.WriteLine ("your autosendcmd = " + ascmd);

			//Console.WriteLine ("packnum start: " + packnum);
			//Console.WriteLine("botname is: " + botname);

			replaceConfig(content, posofstartcmd, posofendcmd, packnumb, bot, epname);



		}

		public static void replaceConfig(string oldcontent, int ascmdposstart, int ascmdposend, int packnumnew, string bot, string epname){
			string logfile = "logfile.txt";
			try{

				if (!File.Exists (logfile)) {
					File.WriteAllText (logfile, "");
				}
			} catch {
				Console.WriteLine ("could not create logfile.txt, manually creating could solve the problem!");
			}

			string logcontent = String.Empty;
			bool Continue = true;
			try{
				var logfileopen = new FileStream(logfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				var lcontent = new StreamReader(logfileopen, Encoding.Default);
				logcontent = lcontent.ReadToEnd ();
			}catch{
				Console.WriteLine ("could not open logfile, its either not created or something else went wrong!");
			}
			string[] packbot = logcontent.Split ('^');
			string newpackbot = bot + "@" + packnumnew;
			if (logcontent.Contains (newpackbot)) {
				Continue = false;
			} else {
				Continue = true;
			}

			if (Continue == true) {
				 
				string packnumnews = packnumnew.ToString ();
				string newascmd = "autosendcmd = \"/join " + channel + ";/msg " + bot + " xdcc send #" + packnumnews;

				var newConfig = new StringBuilder (oldcontent);
				newConfig.Remove (ascmdposstart, ascmdposend);
				newConfig.Insert (ascmdposstart, newascmd);
				string newConfigString = newConfig.ToString ();

				File.WriteAllText ("config", newConfigString);
				File.AppendAllText (logfile, bot + "@" + packnumnew + "^\n");

				Console.WriteLine ("packnum changed, config will be updated, irssi will launch when coded " + packnumnew);
				Console.WriteLine ("filename = " + epname);

				if (irc_contest) {
					StartIRC (epname);
				}
			} else {

				Console.WriteLine ("pack stayed same as previous, no updated on config needed, irssi wont launch ");
			}



		}

		public static bool FileSizeChanging(string filename){
			long oldlength = 0;
			long length = 0;
			bool run_dl = true;

			while (true) {
				System.Threading.Thread.Sleep (10000);
				FileInfo fi = new FileInfo(@"/home/pi/" + filename);
				length = fi.Length;
				Console.WriteLine ("filesize = " + length);	
				if (length != oldlength) {
					run_dl = true;
				} else {
					run_dl = false;
					Console.WriteLine ("FILE IS FOUND AND CREATED, DOWNLOAD IS FINISHD @ " + GetTimestamp());
					break;
				}

				oldlength = length;

			}

			return run_dl;

		}

		public static bool irc_contest = true;

		public static void StartIRC(string filename){


			System.Threading.Thread.Sleep (5000);
			System.Diagnostics.Process proc = new System.Diagnostics.Process ();
			proc.EnableRaisingEvents = false; 
			proc.StartInfo.FileName = "irssi";
			proc.Start ();
			//System.Threading.Thread.Sleep (sleep);

			long oldlength = 0;
			long length = 0;

			int x = 0;

			while (true) {
				System.Threading.Thread.Sleep (10000);
				x++;
				FileInfo fi = new FileInfo(@"/home/pi/" + filename);
				bool exists = fi.Exists;
				length = fi.Length;



				if (length == oldlength) {
					Console.WriteLine ("FILE IS FOUND AND CREATED, DOWNLOAD IS FINISHD @ " + GetTimestamp());

					if (x < 5) {
						irc_contest = false;
					} else {
						proc.Kill ();
						System.Threading.Thread.Sleep (10000);
						break;
					}

				}
					

				oldlength = length;

			}



		}

		public static String GetTimestamp()
		{
			DateTime CurrentDate;
			CurrentDate = DateTime.Now;
			return CurrentDate.ToString ();
		}

	}
}

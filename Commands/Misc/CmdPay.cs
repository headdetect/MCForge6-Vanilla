﻿/*
Copyright 2011 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Utils;

namespace MCForge.Commands.Misc
{
    class CmdPay : ICommand
    {
        public string Name { get { return "Pay"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Sinjai"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }
        public void Use(Player p, string[] args)
        {
            if (args.Length != 2) { Help(p); return; }
            Player who = Player.Find(args[0]);
            if (who == null) { p.SendMessage("Could not find \"" + args[0] + "\"!"); return; }
            if (who == p && !p.IsOwner) { p.SendMessage("You cannot pay yourself!"); return; }
            int amt;
            try { amt = int.Parse(args[1]); }
            catch { p.SendMessage("Invalid amount!"); return; }
            who.ExtraData.CreateIfNotExist("Money", 0);
            p.ExtraData.CreateIfNotExist("Money", 0);
            if ((int)p.ExtraData["Money"] - amt < 0) { p.SendMessage("You cannot pay with more " + Server.Moneys + " than you have!"); return; }
            if ((int)who.ExtraData["Money"] + amt > 16777215) { p.SendMessage("You cannot pay that much! " + who.Color + who.Username + Server.DefaultColor + " cannot have over 16777215 " + Server.Moneys + "."); return; }
            if (amt < 0) { p.SendMessage("Cannot give negative amounts of " + Server.Moneys + "."); return; }
            p.ExtraData["Money"] = (int)p.ExtraData["Money"] - amt;
            who.ExtraData["Money"] = (int)who.ExtraData["Money"] + amt;
            Player.UniversalChat(p.Color + p.Username + Server.DefaultColor + " was paid &3" + amt + Server.DefaultColor + " " + Server.Moneys + " by " + who.Color + who.Username + Server.DefaultColor + ".");
        }
        public void Help(Player p)
        {
            p.SendMessage("/pay <player> <amount> - Pay <player> <amount> of " + Server.Moneys + ".");
        }
        public void Initialize()
        {
            Command.AddReference(this, "pay");
        }
    }
}
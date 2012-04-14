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
using System.Globalization;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.World;

namespace CommandDll
{
    public class CmdMode : ICommand
    {
        public string Name { get { return "Mode"; } }
        public CommandTypes Type { get { return CommandTypes.misc; } }
        public string Author { get { return "Arrem"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0) { Help(p); return; }
            if (!p.mode)
            {
                Block b = Block.FindByName(args[0]);
                if (b == null) { p.SendMessage("Cannot find block\"" + args[0] + "\"!"); return; }
                if (b == Block.FindByName("air")) { p.SendMessage("Cannot use air mode!"); return; }
                if (b.Permission > p.group.permission) { p.SendMessage("Cannot place " + Up(b.Name) + "!"); return; }
                p.mode = true;
                p.modeblock = b;
                p.SendMessage("&b" + Up(b.Name) + Server.DefaultColor + " mode &9on");
                return;
            }
            else
            {
                if (Block.FindByName(args[0]).Name != p.modeblock.Name)
                {
                    Block b = Block.FindByName(args[0]);
                    if (b == null) { p.SendMessage("Cannot find block\"" + args[0] + "\"!"); return; }
                    if (b == Block.FindByName("air")) { p.SendMessage("Cannot use air mode!"); return; }
                    if (b.Permission > p.group.permission) { p.SendMessage("Cannot place " + Up(b.Name) + "!"); return; }
                    p.mode = true;
                    p.modeblock = b;
                    p.SendMessage("&b" + Up(b.Name) + Server.DefaultColor + " mode &9on");
                    return;
                }
                p.SendMessage("&b" + Up(p.modeblock.Name) + Server.DefaultColor + " mode &coff");
                p.mode = false;
                p.modeblock = null;
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/mode <block> - makes every placed block turn into the block specified");
            p.SendMessage("/<block> will also work");
        }

        public void Initialize()
        {
            Command.AddReference(this, "mode");
        }
        string Up(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
    }
}

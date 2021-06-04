using CodeLock.Models;
using Rocket.API;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Linq;
using UnityEngine;

namespace CodeLock
{
    public class Main : RocketPlugin<Config>
    {
        protected override void Load()
        {
            base.Load();
        }
        protected override void Unload()
        {
            base.Unload();
        }
        [RocketCommand("setpassword", "", "", AllowedCaller.Player)]
        [RocketCommandAlias("setpw")]
        [RocketCommandPermission("mixy.setlock")]
        public void SetLock(IRocketPlayer caller, string[] args)
        {
            var player = caller as UnturnedPlayer;
            var c = Configuration.Instance;
            if (args.Length != 1)
            {
                UnturnedChat.Say(player, "Wrong Usage! Right: /setpw 0000", Color.red);
                return;
            }
            if (uint.TryParse(args[0], out uint password))
            {
                if (password < 1000 || password > 9999)
                {
                    UnturnedChat.Say(player, "Password can only be between 1000 and 9999.", Color.red);
                    return;
                }
                if (Physics.Raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), out var result, 5f, RayMasks.BARRICADE_INTERACT))
                {
                    if (result.transform.TryGetComponent<InteractableDoorHinge>(out var hinge))
                    {
                        if (hinge.door.owner == player.CSteamID)
                        {
                            var doorDb = c.Doors.FirstOrDefault(d => d.InstanceId == result.transform.GetInstanceID());
                            if (doorDb == null)
                            {
                                c.Doors.Add(new Door { InstanceId = result.transform.GetInstanceID(), Owner = player.CSteamID.m_SteamID, Password = password});
                                Configuration.Save();
                                UnturnedChat.Say(player, $"Door is locked. Password: <color=white>{password}</color>", Color.yellow, true);
                            }
                            else
                            {
                                doorDb.Password = password;
                                Configuration.Save();
                                UnturnedChat.Say(player, $"Password changed. New password: <color=white>{password}</color>", Color.yellow, true);
                            }
                        }
                        else
                        {
                            UnturnedChat.Say(player, "You don't own this door.", Color.red);
                        }
                    }
                    else
                    {
                        UnturnedChat.Say(player, "Door not found.", Color.red);
                    }
                }
                else
                {
                    UnturnedChat.Say(player, "Object not found.", Color.red);
                }
            }
            else
            {
                UnturnedChat.Say(player, "Password can only be number.", Color.red);
            }           
        }
        [RocketCommand("opendoor", "", "", AllowedCaller.Player)]
        [RocketCommandAlias("od")]
        [RocketCommandPermission("mixy.opendoor")]
        public void OpedLock(IRocketPlayer caller, string[] args)
        {
            var player = caller as UnturnedPlayer;
            var c = Configuration.Instance;
            if (args.Length != 1)
            {
                UnturnedChat.Say(player, "Wrong Usage! Right: /opendoor 0000", Color.red);
                return;
            }
            if (uint.TryParse(args[0], out uint password))
            {
                if (password < 1000 || password > 9999)
                {
                    UnturnedChat.Say(player, "Password can only be between 1000 and 9999.", Color.red);
                    return;
                }
                if (Physics.Raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), out var result, 5f, RayMasks.BARRICADE_INTERACT))
                {
                    if (result.transform.TryGetComponent<InteractableDoorHinge>(out var hinge))
                    {
                        if (!hinge.door.isOpen)
                        {
                            var doorDb = c.Doors.FirstOrDefault(d => d.InstanceId == result.transform.GetInstanceID());
                            if (doorDb != null)
                            {
                                if (password == doorDb.Password)
                                {
                                    BarricadeManager.ServerSetDoorOpen(hinge.door, true);
                                }
                                else
                                {
                                    UnturnedChat.Say(player, "Wrong password.", Color.red);
                                }
                            }
                            else
                            {
                                UnturnedChat.Say(player, "Door is not passworded.", Color.red);
                            }
                        }
                        else
                        {
                            UnturnedChat.Say(player, "Door is already open.", Color.red);
                        }                        
                    }
                    else
                    {
                        UnturnedChat.Say(player, "Door not found.", Color.red);
                    }
                }
                else
                {
                    UnturnedChat.Say(player, "Object not found.", Color.red);
                }
            }
            else
            {
                UnturnedChat.Say(player, "Password can only be number.", Color.red);
            }
        }
    }
}

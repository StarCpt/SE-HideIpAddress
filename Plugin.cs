using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Plugins;
using HarmonyLib;
using SpaceEngineers.Game.GUI;
using VRageMath;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage;
using System.Reflection;
using VRage.Utils;

namespace SE_HideIpAddress
{
    public class Plugin : IPlugin
    {
        public void Init(object gameInstance)
        {
            new Harmony("HideIpAddress").PatchAll(Assembly.GetExecutingAssembly());
        }

        public void Update()
        {

        }

        public void Dispose()
        {

        }

    }

    [HarmonyPatch(typeof(MyGuiScreenMainMenu), "GenerateContinueTooltip")]
    class MainMenuPatch
    {
        [HarmonyAfter]
        static void Postfix(MyObjectBuilder_LastSession lastSession, MyGuiControlBase ___m_continueTooltipcontrol, MyGuiScreenMainMenu __instance)
        {
            try
            {
                if (lastSession.IsOnline)
                {
                    Vector2 vector2 = new Vector2(0.005f, 1f / 500f);
                    MyGuiControlParent imageTooltip = ___m_continueTooltipcontrol as MyGuiControlParent;
                    MyGuiControlLabel control1 = imageTooltip.Controls[1] as MyGuiControlLabel;
                    MyGuiControlBase control2 = imageTooltip.Controls[0];
                    control1.Text = $"{MyTexts.GetString(MyCommonTexts.ToolTipContinueGame)}{Environment.NewLine}{lastSession.GameName}";

                    ___m_continueTooltipcontrol.Size =
                        new Vector2(Math.Max(imageTooltip.Controls[0].Size.X, imageTooltip.Controls[1].Size.X) + vector2.X * 2, ___m_continueTooltipcontrol.Size.Y);
                    control1.Position = -imageTooltip.Size / 2f + vector2;
                    control2.Position = new Vector2(0.0f, imageTooltip.Size.Y / 2f - vector2.Y);
                }
            }
            catch (Exception e)
            {
                MyLog.Default.Error(e.ToString());
                MyGuiSandbox.Show(MyStringId.GetOrCompute("HideIpAddress Plugin Crashed! Report it to the author."));
            }
        }
    }
}

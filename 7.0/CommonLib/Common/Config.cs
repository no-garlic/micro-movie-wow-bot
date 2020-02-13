
using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;

namespace CommonLib
{
    public abstract class Config
    {
        public static int           FrameSize           = 2;
        public static ScreenCorner  Corner              = ScreenCorner.BottomRight;

        public static bool          IgnoreDataChecks    = false;
        public static bool          IgnoreReadyCheck    = false;

        public static bool          ForceFightingABoss  = false;
        public static bool          DisableFocusMode    = false;
        
        public static bool          HaveLegendaryGloves = true;

        public static bool          UseOldFinishingMove = false;

        public static bool          RemapCapslock       = true;

        public static bool          GenerateBindings    = false;

        public static bool          LoggingEnabled      = false;
        public static Log.LogDetail LoggingDetail       = Log.LogDetail.Med;
        public static string        LoggingFolder       = "C:\\Games\\Logs";
    }


}

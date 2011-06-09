using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cortex.OpenFocus
{
    public static class Helper
    {
        public static void Connect()
        {
            try /* Try to connect to the bootloader */
            {
                Bootloader.Connect();
                UInt16 PageSize = Bootloader.PageSize;
                UInt16 FlashSize = Bootloader.FlashSize;

                Logger.Write("Device Found!");
                Logger.Write("Page Size: " + PageSize.ToString() + " bytes");
                Logger.Write("Flash Size: " + FlashSize.ToString() + " bytes");
            }
            catch (DeviceNotFoundException) /* If the device isn't found... */
            {
                try /* Try connecting to the device and rebooting it into the bootloader */
                {
                    Device dev = new Device();
                    dev.Connect();
                    Logger.Write("Rebooting device into firmware update mode...");
                    dev.RebootToBootloader();
                    dev.Disconnect();
                    System.Threading.Thread.Sleep(2000);
                    Connect();
                    return;
                }
                catch (DeviceNotFoundException) /* If this is reach, the device probably not connected */
                {
                    Logger.Write("Device not found!", Logger.LogType.Error);
                    return;
                }
            }
        }

        public static void UploadFile(string file)
        {
            Byte[] data = null;
            UInt16 FlashSize = 0;

            Logger.Write("Attempting to connect to bootloader");

            Helper.Connect();

            try
            {
                IntelHexFile f = IntelHexFile.Open(file);
                f.PageSize = Bootloader.PageSize;
                data = f.Data;

                FlashSize = Bootloader.FlashSize;

                if (data.Length > (FlashSize - 4096))
                {
                    Logger.Write("File is too large!");
                    return;
                }
                Logger.Write("Ready to upload " + data.Length.ToString() + " bytes of data");
            }
            catch (ChecksumMismatchException)
            {
                Logger.Write("Checksum mismatch! File is not valid.");
                return;
            }

            Bootloader.WriteFlash(data);

            Logger.Write("Firmware update complete!");
        }
    }
}

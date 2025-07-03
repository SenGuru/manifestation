namespace Manifest.Common.Constants
{
    public static class ConfigurationConstants
    {
        public const string ConfigurationFileName = "ManifestConfig.json";

        public static class ConfigurationGroup
        {
            public const int BaseConfiguration = 1;
#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members
            public const int CommonConfiguration = 2;
#pragma warning restore S3218 // Inner class members should not shadow outer class "static" or type members
            public const int ManifestWebConfiguration = 3;
            public const int CoordinatorConfiguration = 4;
            public const int ReportServerConfiguration = 5;
            public const int BatchProcessConfiguration = 6;
        }

        public static class CommonConfiguration
        {
            public const string CommonEmailDomains = "YAHOO.COM,HOTMAIL.COM,LIVE.COM,GMAIL.COM,GOOGLEMAIL.COM,AOL.COM,OUTLOOK.COM,ICLOUD.COM,MAIL.COM,ME.COM,MAC.COM,YMAIL.COM,REDIFF.COM,GMX.COM,INBOX.COM,LYCOS.COM,OPERA.COM,HUSHMAIL.COM,FACEBOOK.COM";
        }

        public static class DocViewerConfiguration
        {
            public const string DefaultSupportedDocTypes = ".BIB,.DOC,.DOCX,.XML,.HTML,.ODT,.OTT,.OOXML,.PDB,.PDF,.PSW,.RTF,.SDW,.STW,.SXW,.TEXT,.TXT,.VOR,.XHTML,.ART,.ARW,.AVS,.BMP,.CIN,.CMYK,.CMYKA,.CRW,.CUR,.CUT,.DCM,.DCR,.DCX,.DIB,.DPX,.EMF,.EPDF,.EPI,.EPS,.EPSF,.EPSI,.EPT,.EXR,.FAX,.FITS,.GIF,.GRAY,.HALD,.HRZ,.ICO,.INFO,.JNG,.JP2,.JPC,.JPEG,.JPG,.MAT,.MET,.MIFF,.MONO,.MPC,.MRW,.MSL,.MTV,.MVG,.NEF,.ODD,.ORF,.OTB,.OTG,.P7,.PALM,.PAM,.PBM,.PCD,.PCT,.PCX,.PDB,.PEF,.PFA,.PFB,.PFM,.PGM,.PICON,.PICT,.PIX,.PNG,.PNM,.PPM,.PS,.PSD,.PTIF,.PWP,.RAD,.RAF,.RAS,.RGB,.RGBA,.RLA,.RLE,.SCT,.SFW,.SGI,.STD,.SUN,.SVG,.SVM,.SXD,.TGA,.TIFF,.TIM,.TTF,.UIL,.UYVY,.VICAR,.VIFF,.WBMP,.WMF,.WPG,.X,.X3F,.XBM,.XCF,.XPM,.XWD,.YCBCR,.YCBCRA,.YUV,.CSV,.DBF,.DIF,.ODS,.PTS,.PXL,.SDC,.SLK,.STC,.SXC,.XLS,.XLSX,.XLT,.PPT,.PPTX,.V1,.AMI,.ASC";
        }

        public static class SearchConfiguration
        {
            public const string DefaultDocTypes = "PDF,DOC,TXT";
        }

        public static class DocumentConverter
        {
            public const string LocalLibreOfficeExecutable = @"support-apps/LibreOfficePortable/App/libreoffice/program/soffice.exe";
            public const string LocalLibreOfficeExecutableDev = @"Build/support-apps/LibreOfficePortable/App/libreoffice/program/soffice.exe";
            public const string DefaultWindowsLibreOfficeExecutable = @"C:/Program Files/LibreOffice/program/soffice.exe";
            public const string DefaultLinuxLibreOfficeExecutable = @"C:/Program Files/LibreOffice/program/soffice.exe";
        }
    }
}

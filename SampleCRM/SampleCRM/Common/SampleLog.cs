using NLog;
using NLog.Web;

namespace SampleCRM.Common
{
    /// <summary>
    /// SampleCRM全体のLogger
    /// 想定した例外の出力についてはErrorレベルを使用すること
    /// </summary>
    public static class SampleLog
    {
        // Logger本体
        // Setup().LoadConfigurationFromAppSettings()を使用することでnlog.confgを使い分けることができる
        // appsettings.json と同じように nlog.Development.config 等を作成すれば実行時に対応したconfigファイルを読み込んでくれる
        public static readonly Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

        // Logレベルに応じて別名で呼び出す形とする
        // レベルに応じて付加情報が必要な場合は各関数を修正する

        // 下にラッパーメソッドを用意した
        // しかし、出力される文字列のファイル名と行番号がこのクラスファイルのものになってしまう

        /// <summary>
        /// トレースレベル(Level:0)
        /// </summary>
        /// <param name="message"></param>
        public static void Trace( string message ) => logger.Trace( message );

        /// <summary>
        /// トレースレベル(Level:0) using specified parameters
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Trace( string message, params object[] args ) => logger.Trace( message, args );

        /// <summary>
        /// デバッグレベル(Level:1)
        /// </summary>
        /// <param name="message"></param>
        public static void Debug( string message ) => logger.Debug( message );

        /// <summary>
        /// デバッグレベル(Level:1) using specified parameters
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Debug( string message, params object[] args ) => logger.Debug( message, args );

        /// <summary>
        /// 情報レベル(Level:2)
        /// </summary>
        /// <param name="message"></param>
        public static void Info( string message ) => logger.Info( message );

        /// <summary>
        /// 情報レベル(Level:2) using specified parameters
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Info( string message, params object[] args ) => logger.Info( message, args );

        /// <summary>
        /// 警告レベル(Level:3)
        /// </summary>
        /// <param name="message"></param>
        public static void Warn( string message ) => logger.Warn( message );

        /// <summary>
        /// 警告レベル(Level:3) using specified parameters
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Warn( string message, params object[] args ) => logger.Warn( message, args );

        /// <summary>
        /// エラーレベル(Level:4)
        /// </summary>
        /// <param name="message"></param>
        public static void Error( string message ) => logger.Error( message );

        /// <summary>
        /// エラーレベル(Level:4) using specified parameters
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Error( string message, params object[] args ) => logger.Error( message, args );

        /// <summary>
        /// エラーレベル(Level:4)
        /// 例外発生時の内容取得用
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        public static void Error( Exception exception, string message ) => logger.Error( exception, message );

        /// <summary>
        /// エラーレベル(Level:4) using specified parameters
        /// 例外発生時の内容取得用
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Error( Exception exception, string message, params object[] args ) => logger.Trace( exception, message, args );

        /// <summary>
        /// 致命的レベル(Level:5)
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal( string message ) => logger.Fatal( message );

        /// <summary>
        /// 致命的レベル(Level:5) using specified parameters
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Fatal( string message, params object[] args ) => logger.Fatal( message, args );
    }
}

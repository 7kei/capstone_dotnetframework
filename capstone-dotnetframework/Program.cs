using System;
using System.Data.SqlClient;
using System.IO.Ports;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Linq;
using System.Data;

namespace capstone_dotnetframework
{
    class Globals
    {
        public static SqlConnection connection;
        public static SerialPort globalSerialPort;
        public static bool isFallbackModeOn = false;
        public static MainForm mainForm;
    }

    class Config
    {
        public string SqlDataSource { get; set; }
        public string SqlUserID { get; set; }
        public string SqlPassword { get; set; }
        public string SqlInitialCatalog { get; set; }
        public string SerialPort { get; set; }

    }

    class Program
    {

        [STAThread]
        public static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Globals.mainForm = new MainForm();
            Application.Run(Globals.mainForm);

        }

        public static void Initialize()
        {
            Globals.mainForm.WriteToOutputWindow("Initializing.");

            // Load Config
            var configFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json").ToString();
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFilePath));

            // Load Serial
            try
            {
                Globals.globalSerialPort = new SerialPort(config.SerialPort)
                {
                    BaudRate = 9600,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    DataBits = 8,
                    Handshake = Handshake.None,
                    RtsEnable = true
                };
                Globals.globalSerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                Globals.globalSerialPort.Open();
            } finally
            {
                Globals.mainForm.WriteToOutputWindow("Serial Port Open!");
            }

            // Connect to SQL
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
                {
                    DataSource = config.SqlDataSource,
                    UserID = config.SqlUserID,
                    Password = config.SqlPassword,
                    InitialCatalog = config.SqlInitialCatalog
                };

                Globals.connection = new SqlConnection(builder.ConnectionString);
                Globals.connection.Open();
            }
            finally
            {
                Globals.mainForm.WriteToOutputWindow("SQL Connected!");
            }

        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();

            if (indata.StartsWith("READ_") && !Globals.isFallbackModeOn)
                readCardInfo(indata);
        }

        private static void readCardInfo(string str)
        {
            string[] splitString = str.Replace("\r", string.Empty).Split('_');

            var roomId = splitString[1];
            var cardId = splitString[2];

            var roomMinAccess = 0;

            var userFullName = "";
            var userAccessLevel = 0;
            var userId = "";
            var userGradeLevelSection = "";

            string sqlGetRoomCommand = $"SELECT MinAccessLevel FROM UserDatabase.dbo.RoomTable WHERE RoomID={roomId}";
            string sqlGetUserCommand = $"SELECT UserFullName,UserAccessLevel,UserID,UserGradeLevelSection "
                                     + $"FROM UserDatabase.dbo.UserTable WHERE UserID='{cardId}'";

            // Get room
            using (SqlCommand command = new SqlCommand(sqlGetRoomCommand, Globals.connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        roomMinAccess = reader.GetInt32(0);
                }
            }

            // Get user
            using (SqlCommand command = new SqlCommand(sqlGetUserCommand, Globals.connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Globals.globalSerialPort.WriteLine($"REPLY_DENIED_Unknown User");
                        return;
                    }

                    while (reader.Read())
                    {
                        userFullName = reader.GetString(0);
                        userAccessLevel = reader.GetInt32(1);
                        userId = reader.GetString(2);
                        userGradeLevelSection = reader.GetString(3);
                        Globals.mainForm.WriteToOutputWindow($"UserFullName: {userFullName}, "
                                                            +$"UserAccessLevel: {userAccessLevel}, "
                                                            +$"UserID: {userId}, "
                                                            +$"UserGradeLevelSection: {userId}");
                    }
                }
                
            }

            if (userAccessLevel >= roomMinAccess)
            {
                Globals.globalSerialPort.WriteLine($"REPLY_PERMITTED_{userFullName}");
                Globals.mainForm.WriteToOutputWindow($"REPLY_PERMITTED_{userFullName}");
                writeAttendance(userFullName, roomId, userId, userGradeLevelSection);
            } 
            else
            {
                Globals.globalSerialPort.WriteLine($"REPLY_DENIED_{userFullName}");
                Globals.mainForm.WriteToOutputWindow($"REPLY_DENIED_{userFullName}");
            }
        }

        private static void writeAttendance(string userName, string roomIdEntered, string userId, string userGradeLevelSection)
        {

            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            var currentTime = DateTime.Now.ToString("HH:mm:ss");

            string sqlCommand = $"INSERT INTO UserDatabase.dbo.AttendanceTable "
                              + $"(Date, Time, StudentID, Name, GradeAndSection, RoomIdEntered) "
                              + $"VALUES ('{currentDate}', '{currentTime}', '{userId}', "
                              + $"'{userName}', '{userGradeLevelSection}', {roomIdEntered})";

            using (SqlCommand command = new SqlCommand(sqlCommand, Globals.connection))
            {
                command.ExecuteNonQuery();
            }

            Globals.globalSerialPort.WriteLine($"REPLY_ATTENDED_{userName}");

        }

        public static void turnOnFallbackMode()
        {
            Globals.mainForm.WriteToOutputWindow("FALLBACK MODE ON!");

            Globals.globalSerialPort.WriteLine($"REPLY_FALLBACKMODE_ON");
            Globals.isFallbackModeOn = true;
        }

        public static void turnOffFallbackMode()
        {
            Globals.mainForm.WriteToOutputWindow("FALLBACK MODE OFF!");

            Globals.globalSerialPort.WriteLine($"REPLY_FALLBACKMODE_OFF");
            Globals.isFallbackModeOn = false;
        }

        public static void ToCsv(DataTable table, string fileName, string colSep = ",", string rowSep = "\n")
        {
            var format = string.Join(colSep, Enumerable.Range(0, table.Columns.Count)
                                                    .Select(i => string.Format("{{{0}}}", i)));

            var d = "Date,Time,Student ID,Name,Grade And Section,Room ID Entered\n";

            var e = string.Join(rowSep, table.Rows.OfType<DataRow>()
                                                .Select(i => string.Format(format, i.ItemArray)));

            File.WriteAllText(fileName, d+e);
        }

        public static void exportAttendanceFile(string fileName, string date, string section="")
        {
            string sqlGetCurrentDayAttendance = $"SELECT * FROM UserDatabase.dbo.AttendanceTable WHERE Date='{date}'";

            if (section != "")
            {
                sqlGetCurrentDayAttendance = $"SELECT * FROM UserDatabase.dbo.AttendanceTable "
                                           + $"WHERE Date='{date}' AND GradeAndSection='{section}'";
            }

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.TableMappings.Add("Table", "AttendanceTable");
            DataSet dataSet = new DataSet("AttendanceTable");
            SqlCommand command = new SqlCommand(sqlGetCurrentDayAttendance);
            command.CommandType = CommandType.Text;
            adapter.SelectCommand = command;
            adapter.SelectCommand.Connection = Globals.connection;
            adapter.Fill(dataSet);

            ToCsv(dataSet.Tables[0], fileName);

        }

    }
}

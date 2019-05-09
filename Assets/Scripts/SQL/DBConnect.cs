using MySql.Data.MySqlClient;
using UnityEngine;

public class DBConnect : UnitySingleton<DBConnect>
{

    public static MySqlConnection dbConnection;
    static string Host = "127.0.0.1";
    static string Id = "root";
    static string Pwd = "123456";
    static string Database = "shepherd";

    // Start is called before the first frame update
    void Start()
    {
        // 连接数据库
        string connectionString = string.Format("Server = {0}; Database = {1}; User ID = {2}; Password = {3};", Host, Database, Id, Pwd);
        dbConnection = new MySqlConnection(connectionString);
        dbConnection.Open();
    }


    public void InsertIntoFinishStep(int num, int step)
    {
        string cmd = $"insert into finish_step(num, step) values({num}, {step})";
        ExcuteCmd(cmd, "插入表 finish_time 成功");
    }

    public void InsertIntoDistanceTest(float dis, int step)
    {
        string cmd = $"insert into distance_test(dis, step) values({dis}, {step})";
        ExcuteCmd(cmd, "插入表 distance_test 成功");
    }

    public void InsertIntoRaTest(float ra, int step)
    {
        string cmd = $"insert into ra_test(ra, step) values({ra},{step})";
        ExcuteCmd(cmd, "插入表 ra_test 成功");
    }

    public void InsertIntoShepherdAction(int num, float collect, float drive)
    {
        string cmd = $"insert into shepherd_action(num, collect, drive) values({num}, {collect}, {drive})";
        ExcuteCmd(cmd, "插入表 shepherd_action 成功");
    }

    public void InsertIntoRandomGenerateTest(float length, int step)
    {
        string cmd = $"insert into random_generate_test(length, step) values({length},{step})";
        ExcuteCmd(cmd, "插入表 random_generate_test 成功");
    }

    private void ExcuteCmd(string sql, string msg = "Insert success")
    {
        MySqlCommand mycmd = new MySqlCommand(sql, dbConnection);
        if (mycmd.ExecuteNonQuery() > 0)
            Debug.Log(msg);
    }
}

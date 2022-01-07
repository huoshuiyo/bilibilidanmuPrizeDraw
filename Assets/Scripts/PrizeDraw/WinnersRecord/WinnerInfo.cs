using SQLite4Unity3d;

public class WinnerInfo
{
    public string Uid { get; set; }

    public string Prize { get; set; }

    public int IsExcluded { get; set; }

    public string Time { get; set; }

    public override string ToString()
    {
        return string.Format("[User: Uid={0}, Prize={1}, IsExcluded={2}, Time={3} ]", this.Uid, this.Prize, this.IsExcluded,this.Time);
    }
}



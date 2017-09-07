

/// <summary>
/// 事件ID
/// </summary>
public class MsgConst
{

	// ----------- 资源更新(1000-1099) -----------
	public const int Update_Message = 1000;           //更新消息
    public const int Update_Extract = 1001;           //更新解包
    public const int Update_Download = 1002;         //更新下载
    public const int Update_Progress = 1003;         //更新进度
	public const int Resource_Ready = 1004;         //资源更新完毕（可以开始加载资源了）
	// ----------------------
	


	// ----------- 启动游戏(1100-1199) -----------
	public const int Launch_Step = 1100;
	public const int Launch_Percent = 1101;
	public const int Launch_Done = 1102;
	// ----------------------



	// ----------- 角色(2000-2999) -----------
	public const int Actor_Spawn = 2000;
	public const int Actor_PositionChanged = 2001;      // 位置改变
	public const int Actor_MoveBegin = 2002;     // 停止移动
	public const int Actor_MoveStop = 2003;     // 停止移动
	public const int Actor_ManualMove = 2004;	// 摇杆移动、直线移动
	public const int Actor_Die = 2005;
	public const int Actor_Revive = 2006;
	public const int Actor_DizzyBegin = 2007;
	public const int Actor_DizzyEnd = 2008;
	public const int Actor_LevitateBegin = 2009;
	public const int Actor_LevitateEnd = 2010;
	public const int Actor_TauntBegin = 2011;
	public const int Actor_TauntEnd = 2012;
	public const int Actor_SpeedChanged = 2013;
	public const int Actor_Hit = 2014;				// 被打
	public const int Actor_Kill = 2015;             // 杀死别人
	public const int Actor_StealthBegin = 2016;
	public const int Actor_StealthEnd = 2017;
	public const int Actor_Destroy = 2017;			// 真死

	public const int Actor_AttackBegin = 2040;
	public const int Actor_AttackEnd = 2041;
	// 下列技能事件仅限主动技
	public const int Actor_SingBegin = 2042;
	public const int Actor_SingEnd = 2043;
	public const int Actor_PreBegin = 2044;
	public const int Actor_PreEnd = 2045;
	public const int Actor_ReleaseBegin = 2046;
	public const int Actor_ReleaseEnd = 2047;
	public const int Actor_PostBegin = 2048;
	public const int Actor_PostEnd = 2049;
	public const int Actor_SkillBegin = 2050;
	public const int Actor_SkillEnd = 2051;

	public const int Actor_next0000 = 2080;
	// ----------------------



	// ----------- 关卡(3000-3999) -----------
	public const int Stronghold_A_Occupied = 3000;
	public const int Stronghold_B_Occupied = 3001;
	public const int Statue_Destroyed = 3002;
    // ----------------------


    // ------------ UI ---------------------
    public const int UI_Event = 4000;
    // ----------------------------



}

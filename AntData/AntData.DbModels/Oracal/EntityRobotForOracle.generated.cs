using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AntData.ORM;
using AntData.ORM.Linq;
using AntData.ORM.Mapping;

namespace DbModels.Oracle
{
	/// <summary>
	/// Database       : orcl
	/// Data Source    : dbtest
	/// Server Version : 11.2.0.1.0
	/// </summary>
	public partial class Entitys : IEntity
	{
		/// <summary>
		/// �û���
		/// </summary>
		public IQueryable<Person> People  { get { return this.Get<Person>(); } }
		/// <summary>
		/// ѧУ��
		/// </summary>
		public IQueryable<School> Schools { get { return this.Get<School>(); } }

		private readonly IDataContext con;

		public IQueryable<T> Get<T>()
			 where T : class
		{
			return this.con.GetTable<T>();
		}

		public Entitys(IDataContext con)
		{
			this.con = con;
		}
	}

	/// <summary>
	/// �û���
	/// </summary>
	[Table(Schema="TEST", Comment="�û���", Name="PERSON")]
	public partial class Person : BaseEntity
	{
		#region Column

		/// <summary>
		/// ����
		/// </summary>
		[SequenceName("Oracle", "PERSONSEQ", SequenceFunction = "GET_PERSONSEQ_IDENTITY_ID"), Column("ID",                  DataType=DataType.Decimal,  Length=22, Precision=15, Scale=0, Comment="����"), PrimaryKey, Identity]
		public long Id { get; set; } // NUMBER (15,0)

		/// <summary>
		/// ����
		/// </summary>
		[Column("NAME",                DataType=DataType.VarChar,  Length=50, Comment="����"), NotNull]
		public string Name { get; set; } // VARCHAR2(50)

		/// <summary>
		/// ���
		/// </summary>
		[Column("AGE",                 DataType=DataType.Decimal,  Length=22, Precision=5, Scale=0, Comment="���"),    Nullable]
		public int? Age { get; set; } // NUMBER (5,0)

		/// <summary>
		/// School���
		/// </summary>
		[Column("SCHOOLID",            DataType=DataType.Decimal,  Length=22, Precision=15, Scale=0, Comment="School���"),    Nullable]
		public long? Schoolid { get; set; } // NUMBER (15,0)

		/// <summary>
		/// ������ʱ��
		/// </summary>
		[Column("DATACHANGE_LASTTIME", DataType=DataType.DateTime, Length=7, Comment="������ʱ��"), NotNull]
		public DateTime DatachangeLasttime // DATE
		{
			get { return _DatachangeLasttime; }
			set { _DatachangeLasttime = value; }
		}

		#endregion

		#region Field

		private DateTime _DatachangeLasttime = System.Data.SqlTypes.SqlDateTime.MinValue.Value;

		#endregion

		#region Associations

		/// <summary>
		/// persons_school
		/// </summary>
		[Association(ThisKey="Schoolid", OtherKey="Id", CanBeNull=true, KeyName="persons_school", BackReferenceName="personsschools")]
		public School Personsschool { get; set; }

		#endregion
	}

	/// <summary>
	/// ѧУ��
	/// </summary>
	[Table(Schema="TEST", Comment="ѧУ��", Name="SCHOOL")]
	public partial class School : BaseEntity
	{
		#region Column

		/// <summary>
		/// ����
		/// </summary>
		[SequenceName("Oracle", "SCHOOLSEQ", SequenceFunction = "GET_SCHOOLSEQ_IDENTITY_ID"), Column("ID",                  DataType=DataType.Decimal,  Length=22, Precision=15, Scale=0, Comment="����"), PrimaryKey, Identity]
		public long Id { get; set; } // NUMBER (15,0)

		/// <summary>
		/// ѧУ����
		/// </summary>
		[Column("NAME",                DataType=DataType.VarChar,  Length=50, Comment="ѧУ����"),    Nullable]
		public string Name { get; set; } // VARCHAR2(50)

		/// <summary>
		/// ѧУ��ַ
		/// </summary>
		[Column("ADDRESS",             DataType=DataType.VarChar,  Length=100, Comment="ѧУ��ַ"),    Nullable]
		public string Address { get; set; } // VARCHAR2(100)

		/// <summary>
		/// ������ʱ��
		/// </summary>
		[Column("DATACHANGE_LASTTIME", DataType=DataType.DateTime, Length=7, Comment="������ʱ��"), NotNull]
		public DateTime DatachangeLasttime // DATE
		{
			get { return _DatachangeLasttime; }
			set { _DatachangeLasttime = value; }
		}

		#endregion

		#region Field

		private DateTime _DatachangeLasttime = System.Data.SqlTypes.SqlDateTime.MinValue.Value;

		#endregion

		#region Associations

		/// <summary>
		/// persons_school_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="Schoolid", CanBeNull=true, IsBackReference=true)]
		public IEnumerable<Person> Personsschools { get; set; }

		#endregion
	}

	public static partial class TableExtensions
	{
		public static Person FindByBk(this IQueryable<Person> table, long Id)
		{
			return table.FirstOrDefault(t =>
				t.Id == Id);
		}

		public static async Task<Person> FindByBkAsync(this IQueryable<Person> table, long Id)
		{
			return await table.FirstOrDefaultAsync(t =>
				t.Id == Id);
		}

		public static School FindByBk(this IQueryable<School> table, long Id)
		{
			return table.FirstOrDefault(t =>
				t.Id == Id);
		}

		public static async Task<School> FindByBkAsync(this IQueryable<School> table, long Id)
		{
			return await table.FirstOrDefaultAsync(t =>
				t.Id == Id);
		}
	}
}

//�����sql�Ǵ���SEQUENCE
//CREATE SEQUENCE PERSONSEQ;
//CREATE SEQUENCE SCHOOLSEQ; 

//�����sql�Ǵ���һ����ȡָ����Seq�Ľ����function������PERSONSEQ���Ҵ�����seq
//CREATE
//OR REPLACE FUNCTION GET_PERSONSEQ_IDENTITY_ID RETURN NUMBER AS num NUMBER ;
//BEGIN
//	SELECT
//		PERSONSEQ.nextval INTO num
//	FROM
//		dual ; RETURN num ;
//	END GET_PERSONSEQ_IDENTITY_ID ;

//CREATE
//OR REPLACE FUNCTION GET_SCHOOLSEQ_IDENTITY_ID RETURN NUMBER AS num NUMBER ;
//BEGIN
//	SELECT
//		PERSONSEQ.nextval INTO num
//	FROM
//		dual ; RETURN num ;
//	END GET_SCHOOLSEQ_IDENTITY_ID ;

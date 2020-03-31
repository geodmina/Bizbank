using System;
using System.Collections.Generic;

namespace BIZBANK.V2020.CAD.ORACLE.Modelos
{

	public class PackageTransaction
	{

		public string Name;
		public string TranId;
		public string SessionId;
		public string UserId;
		public string CompanyId;
		public string UserType;
		public string ServiceId;
		public string OptionId;
		public string StationId;
		public string BankId;
		public string LoginId;
		public string Source;
		public short ExecType;
		public short ExecMode;
		public List<PackageParameter> Parameters;

		public PackageTransaction()
		{
			Parameters = new List<PackageParameter>();
		}

	}

	public class PackageParameter
	{

		public string Name;
		public int Size;
		public string Value;
		public short Type;
		public short Direction;
		public bool Flag;

		public PackageParameter()
		{

		}

		public PackageParameter(string Name, int Size, string Value, short Type, short Direction, bool Flag)
		{

			this.Name = Name;
			this.Size = Size;
			this.Value = Value;
			this.Type = Type;
			this.Direction = Direction;
			this.Flag = Flag;

		}

	}

}

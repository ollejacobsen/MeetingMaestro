using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingMaestro.Core.Import.Models
{
	public class ImportedEmployee : IEquatable<ImportedEmployee>
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public ImportedEmployee(string id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ImportedEmployee);
		}

		public bool Equals(ImportedEmployee other)
		{
			return other != null && other.Id == this.Id;
		}

		public static (bool valid, ImportedEmployee employee) ValidateAndMap(string[] content)
		{
			var id = string.IsNullOrWhiteSpace(content[0]) ? null : content[0];
			if (id != null)
			{
				return (true, new ImportedEmployee(id, content[1]));
			}

			return (false, null);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPascalCoin.Processing {
	public interface ITransactionalRepository {
		IDisposable BeginTransaction();
		IDisposable Commit();
		IDisposable Rollback();
	}
}

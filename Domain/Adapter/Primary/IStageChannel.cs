using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Adapter.Primary
{
	public interface IStageChannel
	{
		Task<ServiceResultDto> EditTopicAsync();
	}
}

using Domain.DTO;
using System.Threading.Tasks;

namespace Domain.Services.Interface
{
	public interface IStageChannelService
	{
		Task<ServiceResultDto> EditTopicAsync();
	}
}

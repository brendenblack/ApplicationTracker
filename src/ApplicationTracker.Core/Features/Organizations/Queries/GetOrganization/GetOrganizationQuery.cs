using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.Organizations.GetOrganization
{
    public class GetOrganizationQuery : IRequest<GetOrganizationVm>
    {
    }

    public class GetOrganizationHandler : IRequestHandler<GetOrganizationQuery, GetOrganizationVm>
    {
        public Task<GetOrganizationVm> Handle(GetOrganizationQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

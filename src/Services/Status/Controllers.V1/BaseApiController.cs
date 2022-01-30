﻿using Microsoft.AspNetCore.Mvc;

namespace Status.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseApiController : ControllerBase
    {  
    }
}

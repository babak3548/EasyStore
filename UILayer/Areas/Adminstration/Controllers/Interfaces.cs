using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.Areas.Adminstration.Controllers
{
    public interface ICURDOperation<TEntity>
    {
        ActionResult Edit(TEntity entity);
    }
}
using ASPNETWebApp48.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETWebApp48.ViewModels
{
	public class OrderWalkInViewModel
	{
		public List<OrderWalkIn> OrderWalkInLookUp { get; set; }
		public List<OrderItemWalkIn> OrderItemWalkIns { get; set; }
	}
}
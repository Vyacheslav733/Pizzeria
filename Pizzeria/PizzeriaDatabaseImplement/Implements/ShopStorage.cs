using Microsoft.EntityFrameworkCore;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using PizzeriaDatabaseImplement.Models;

namespace PizzeriaDatabaseImplement.Implements
{
    public class ShopStorage : IShopStorage
    {
        public List<ShopViewModel> GetFullList()
        {
            using var context = new PizzeriaDatabase();
            return context.Shops.Include(x => x.Pizzas).ThenInclude(x => x.Pizza).ToList().
                Select(x => x.GetViewModel).ToList();
        }

        public List<ShopViewModel> GetFilteredList(ShopSearchModel model)
        {
            if (string.IsNullOrEmpty(model.ShopName))
            {
                return new();
            }
            using var context = new PizzeriaDatabase();
            return context.Shops.Include(x => x.Pizzas).ThenInclude(x => x.Pizza).Where(x => x.ShopName.Contains(model.ShopName)).
                ToList().Select(x => x.GetViewModel).ToList();
        }

        public ShopViewModel? GetElement(ShopSearchModel model)
        {
            if (string.IsNullOrEmpty(model.ShopName) && !model.Id.HasValue)
            {
                return new();
            }
            using var context = new PizzeriaDatabase();
            return context.Shops.Include(x => x.Pizzas).ThenInclude(x => x.Pizza)
                .FirstOrDefault(x =>
                (!string.IsNullOrEmpty(model.ShopName) && x.ShopName == model.ShopName) ||
                (model.Id.HasValue && x.Id == model.Id))?.GetViewModel;
        }

        public ShopViewModel? Insert(ShopBindingModel model)
        {
            using var context = new PizzeriaDatabase();
            var newShop = Shop.Create(context, model);
            if (newShop == null)
            {
                return null;
            }
            context.Shops.Add(newShop);
            context.SaveChanges();
            return newShop.GetViewModel;
        }

        public ShopViewModel? Update(ShopBindingModel model)
        {
            using var context = new PizzeriaDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var shop = context.Shops.FirstOrDefault(x => x.Id == model.Id);
                if (shop == null)
                {
                    return null;
                }
                shop.Update(model);
                context.SaveChanges();
                shop.UpdatePizzas(context, model);
                transaction.Commit();
                return shop.GetViewModel;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public ShopViewModel? Delete(ShopBindingModel model)
        {
            using var context = new PizzeriaDatabase();
            var shop = context.Shops.Include(x => x.Pizzas).FirstOrDefault(x => x.Id == model.Id);
            if (shop != null)
            {
                context.Shops.Remove(shop);
                context.SaveChanges();
                return shop.GetViewModel;
            }
            return null;
        }

        public bool RestockingShops(SupplyBindingModel model)
        {
            using var context = new PizzeriaDatabase();
            var transaction = context.Database.BeginTransaction();
            var Shops = context.Shops.Include(x => x.Pizzas).ThenInclude(x => x.Pizza).ToList().
                Where(x => x.PizzaMaxCount > x.ShopPizzas.Select(x => x.Value.Item2).Sum()).ToList();
            if (model == null)
            {
                return false;
            }
            try
            {
                foreach (Shop shop in Shops)
                {
                    int difference = shop.PizzaMaxCount - shop.ShopPizzas.Select(x => x.Value.Item2).Sum();
                    int refill = Math.Min(difference, model.Count);
                    model.Count -= refill;
                    if (shop.ShopPizzas.ContainsKey(model.PizzaId))
                    {
                        var datePair = shop.ShopPizzas[model.PizzaId];
                        datePair.Item2 += refill;
                        shop.ShopPizzas[model.PizzaId] = datePair;
                    }
                    else
                    {
                        var pizza = context.Pizzas.First(x => x.Id == model.PizzaId);
                        shop.ShopPizzas.Add(model.PizzaId, (pizza, refill));
                    }
                    shop.PizzasDictionatyUpdate(context);
                    if (model.Count == 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                }
                transaction.Rollback();
                return false;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public bool Sale(SupplySearchModel model)
        {
            using var context = new PizzeriaDatabase();
            var transaction = context.Database.BeginTransaction();
            try
            {
                var shops = context.Shops.Include(x => x.Pizzas).ThenInclude(x => x.Pizza).ToList().
                    Where(x => x.ShopPizzas.ContainsKey(model.PizzaId.Value)).OrderByDescending(x => x.ShopPizzas[model.PizzaId.Value].Item2).ToList();

                foreach (var shop in shops)
                {
                    int residue = model.Count.Value - shop.ShopPizzas[model.PizzaId.Value].Item2;
                    if (residue > 0)
                    {
                        shop.ShopPizzas.Remove(model.PizzaId.Value);
                        shop.PizzasDictionatyUpdate(context);
                        context.SaveChanges();
                        model.Count = residue;

                    }
                    else
                    {
                        if (residue == 0)
                            shop.ShopPizzas.Remove(model.PizzaId.Value);
                        else
                        {
                            var dataPair = shop.ShopPizzas[model.PizzaId.Value];
                            dataPair.Item2 = -residue;
                            shop.ShopPizzas[model.PizzaId.Value] = dataPair;
                        }

                        shop.PizzasDictionatyUpdate(context);
                        transaction.Commit();
                        return true;
                    }
                }
                transaction.Rollback();
                return false;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

    }
}

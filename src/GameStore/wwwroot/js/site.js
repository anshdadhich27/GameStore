var _app;

Storage.prototype.setObject = function (key, value) {
    this.setItem(key, ko.toJSON(value));
}

Storage.prototype.getObject = function (key) {
    var value = this.getItem(key);
    return value && JSON.parse(value);
}

function GetWithoutSpecialChar(str) { return str.replace(/[^0-9a-zA-Z]+/g, '-'); }

if (!String.prototype.trim) { String.prototype.trim = function () { return this.replace(/^\s+|\s+$/g, ''); }; }
if (!String.prototype.Contains) { String.prototype.Contains = function (str) { return this.indexOf(str) !== -1; }; }
if (!String.prototype.startsWith) { String.prototype.startsWith = function (str) { return !this.indexOf(str); }; }
if (!String.prototype.endsWith) { String.prototype.endsWith = function (suffix) { return this.indexOf(suffix, this.length - suffix.length) !== -1; }; }

function isNumber(evt) {
    evt = evt ? evt : window.event;
    var charCode = evt.which ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function isTextOnly(evt) {
    evt = evt ? evt : window.event;
    var charCode = evt.which ? evt.which : evt.keyCode;
    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode === 9) || charCode === 8) {
        return true;
    }
    return false;
}

function isAlphaNumeric(evt) {
    evt = evt ? evt : window.event;
    var charCode = evt.which ? evt.which : evt.keyCode;
    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode === 9) || (charCode > 47 && charCode < 58) || charCode === 32 || charCode === 8 || charCode === 13) {
        return true;
    }
    return false;
}

function isAlphaNumericAndSpecialChar(evt) {
    evt = evt ? evt : window.event;
    var charCode = evt.which ? evt.which : evt.keyCode;
    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode === 9) || (charCode > 47 && charCode < 58) || charCode === 32 || charCode === 45 || charCode === 95 || charCode === 8 || charCode === 46 || charCode === 44 || charCode === 47 || charCode === 92 || charCode === 58 || charCode === 38 || charCode === 37 || charCode === 39) {
        return true;
    }
    return false;
}

function isEmailChar(evt) {
    evt = evt ? evt : window.event;
    console.log(evt);
    var charCode = evt.which ? evt.which : evt.keyCode;
    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode > 47 && charCode < 58) || charCode === 45 || charCode === 95 || charCode === 8 || charCode === 46) {
        return true;
    }
    return false;
}

function isDecimal(evt) {
    evt = evt ? evt : window.event;
    var charCode = evt.which ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode !== 46 && (charCode < 48 || charCode > 57 || charCode === 9))) {
        return false;
    }
    return true;
}

function MainAppViewModel() {
    var self = this;
    self.ShopingCartKey = 'GAME_STORE_SHOPING_CART';
    self.ShopingCartKey_UPDATE = 'GAME_STORE_SHOPING_CART_UPDATE';
    self.SellingCartKey = 'GAME_STORE_SELLING_CART';
    self.SellingCartKey_UPDATE = 'GAME_STORE_SELLING_CART_UPDATE';
    self.PromoCode = ko.observable('');
    self.Tax_Rate = ko.observable(10);
    self.CreditUsed = ko.observable(0);
    self.Credit_Amount = ko.observable(10);
    self.UseCreditAmount = ko.observable(false);
    self.IsUpdating_Cart = ko.observable(false);
    self.Is_Cart_Updated = ko.observable(false);
    self.ShippingCharge = ko.observable(0);
    self.ShippingCharge_Show = ko.computed(function () {
        return parseFloat(self.ShippingCharge()).toFixed(2);
    }, self);
    self.UseCreditAmount.subscribe(function (newValue) {
        if (newValue == false) { self.CreditUsed(0); }
    });
    
    self.ProductTypes = ko.observableArray(["Game", "Console", "Accessories"]);
    self.Selected_ProductType = ko.observable('Game');

    self.Cart = {
        Id: ko.observable(''),
        UserId: ko.observable(0),
        ShopingCart: ko.observableArray([])
    };

    self.Cart2 = {
        Id: ko.observable(''),
        UserId: ko.observable(0),
        SellingCart: ko.observableArray([])
    };

    self.Temp_Selling_Cart = ko.observableArray([]);

    self.Total_Item_In_ShoppingCart = ko.computed(function () {
        return self.Cart.ShopingCart().length;
    }, self);

    self.Total_Item_In_SellingCart = ko.computed(function () {
        return self.Cart2.SellingCart().length;
    }, self);

    self.Set_Cart_Status = function (value) {
        localStorage.setItem(self.ShopingCartKey_UPDATE, value);
    };

    self.Get_Cart_Status = function () {
        return localStorage.getItem(self.ShopingCartKey_UPDATE);
    };

    self.Set_SellingCart_Status = function (value) {
        localStorage.setItem(self.SellingCartKey_UPDATE, value);
    };

    self.Get_SellingCart_Status = function () {
        return localStorage.getItem(self.SellingCartKey_UPDATE);
    };

    self.Total_Price = ko.computed(function () {
        var cart_items = self.Cart.ShopingCart();
        if (cart_items.length === 0) { return 0; }
        var price = 0;
        $.each(cart_items, function (i, o) {
            price += parseFloat(o.TotalPrice());
        });
        return parseFloat(price).toFixed(2);
    }, self);

    self.Total_Price_Sell = ko.computed(function () {
        var cart_items = self.Cart2.SellingCart();
        if (cart_items.length === 0) { return 0; }
        var price = 0;
        $.each(cart_items, function (i, o) {
            price += parseFloat(o.TotalPrice());
        });
        return parseFloat(price).toFixed(2);
    }, self);

    self.Total_Deduction = ko.computed(function () {
        var cart_items = self.Cart2.SellingCart();
        if (cart_items.length === 0) { return 0; }
        var price = 0;
        $.each(cart_items, function (i, o) {
            price += parseFloat(o.Deduction());
        });
        return parseFloat(price).toFixed(2);
    }, self);

    self.Tax_Amount = ko.computed(function () {
        return parseFloat((self.Total_Price() * self.Tax_Rate())/100).toFixed(2);
    }, self);

    self.Tax_Amount_Sell = ko.computed(function () {
        return parseFloat((self.Total_Price_Sell() * self.Tax_Rate())/100).toFixed(2);
    }, self);

    self.Sub_Total = ko.computed(function () {
        var creditSelected = self.UseCreditAmount();
        var amount = parseFloat(self.Total_Price());
        //console.log('Sub_Total: ', amount);
        amount += parseFloat(self.Tax_Amount());
        //console.log('Sub_Total: ', amount);
        amount += parseFloat(self.ShippingCharge());
        //console.log('Sub_Total: ', amount);
        if (Boolean(creditSelected)===true) {
            if (amount >= self.Credit_Amount()) {
                amount -= self.Credit_Amount();
                //console.log('Sub_Total: ', amount);
                self.CreditUsed(self.Credit_Amount());
            } else {
                self.CreditUsed(amount); amount = 0;
                //console.log('Sub_Total: ', amount);
            }
        }
        //console.log('CreditAmount: ', self.CreditUsed());
        return parseFloat(amount).toFixed(2);
    }, self);

    self.Sub_Total_Sell = ko.computed(function () {
        var amount = parseFloat(self.Total_Price_Sell());
        amount -= parseFloat(self.Total_Deduction());
        amount -= parseFloat(self.ShippingCharge());
        return parseFloat(amount).toFixed(2);
    }, self);

    self.Sub_Total_Sell_After_Deduction = ko.computed(function () {
        var amount = parseFloat(self.Total_Price_Sell());
        amount -= parseFloat(self.Total_Deduction());
        //amount -= parseFloat(self.ShippingCharge());
        return parseFloat(amount).toFixed(2);
    }, self);

    self.IsSearching_ForSell = ko.observable(false);
    self.GameList_ForSell = ko.observableArray([]);
    self.ConsoleList_ForSell = ko.observableArray([]);
    self.AccessoriesList_ForSell = ko.observableArray([]);

    self.Search_Key_For_Sell = ko.observable('').extend({ rateLimit: 1000 });
    self.Search_Key_For_Sell.subscribe(function (newValue) {
        newValue = newValue || '';
        if (newValue.length === 0) {
            self.GameList_ForSell.removeAll();
            self.ConsoleList_ForSell.removeAll();
            self.AccessoriesList_ForSell.removeAll();
        } else {
            self.IsSearching_ForSell(true);
            $.ajax({
                type: 'POST', dataType: "json",
                url: '/sell-yours/search',
                data: ko.toJSON({ SearchTxt: newValue, ProductType: self.Selected_ProductType() }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    console.log(data);
                    self.GameList_ForSell.removeAll();
                    self.ConsoleList_ForSell.removeAll();
                    self.AccessoriesList_ForSell.removeAll();
                    switch (self.Selected_ProductType()) {
                        case 'Game':
                            self.GameList_ForSell(data.Games);
                            break;
                        case 'Console':
                            self.ConsoleList_ForSell(data.Consoles);
                            break;
                        case 'Accessories':
                            self.AccessoriesList_ForSell(data.Accessories);
                            break;
                    }
                    self.IsSearching_ForSell(false);
                }
            });
        }
    });
    
    self.Add_To_Shoping_Cart = function (id, typeId, type, price, sku) {
        console.log('Add_To_Shoping_Cart', id, typeId, type, price, sku);
        var product = ko.utils.arrayFirst(self.Cart.ShopingCart(), function (o) {
            return o.Product_Id() === id && o.Product_Type() === type && o.Product_TypeId() === typeId && o.SKU() === sku;
        });
        console.log(product);
        if (product === undefined || product === null) {
            product = new self.ShopingCartViewModel();            
            product.Product_Id(id); product.Product_TypeId(typeId);
            product.Product_Type(type); product.Price(price);            
            product.Quantity(1); product.SKU(sku || '');
            self.Cart.ShopingCart.push(product);
            console.log(ko.toJSON(product));
        }
        self.Set_Cart_Status('false');
        localStorage.setObject(self.ShopingCartKey, self.Cart);
        self.Save_Cart_details_in_Db();
    };

    self.Add_To_Selling_Cart = function (id, typeId, type, price, sku) {
        console.log('Add_To_Selling_Cart', id, typeId, type, price, sku);
        var product = ko.utils.arrayFirst(self.Cart2.SellingCart(), function (o) {
            return o.Product_Id() === id && o.Product_Type() === type && o.Product_TypeId() === typeId && o.SKU() === sku;
        });
        console.log(product);
        if (product === undefined || product === null) {
            product = new self.SellingCartViewModel();
            product.Product_Id(id); product.Product_TypeId(typeId);
            product.Product_Type(type); product.Price(price);
            product.Quantity(1); product.SKU(sku || '');
            self.Cart2.SellingCart.push(product);
            console.log(ko.toJSON(product));
        }
        self.Set_SellingCart_Status('false');
        localStorage.setObject(self.SellingCartKey, self.Cart2);
        self.Save_SellingCart_details_in_Db();
    };

    self.On_Selling_Item_Selected = function (obj) {
        console.log('On_Selling_Item_Selected', obj);
        var _product = ko.utils.arrayFirst(self.Temp_Selling_Cart(), function (o) {
            return o.SKU() === obj.SKU;
        });
        console.log('product:', _product);
        if (_product === undefined || _product === null) {
            //debugger;
            var product = new self.SellingCartViewModel();
            product.Product_Id(obj.Id);
            product.Product_Name(obj.Name);
            product.ImageUrl(obj.ImageUrl);
            product.PageUrl(obj.PageUrl);
            product.Price(obj.Buying_Price);
            product.SKU(obj.SKU);
            product.Quantity(1);

            switch (self.Selected_ProductType()) {
                case 'Game':
                    product.Product_TypeName(obj.PlatformName);
                    product.Product_TypeId(obj.PlatformId);
                    product.Product_Type('Game');
                    break;
                case 'Console':
                    product.Product_TypeName(obj.ProductTypeName);
                    product.Product_TypeId(obj.Product_TypeId);
                    product.Product_Type('Console');
                    break;
                case 'Accessories':
                    product.Product_TypeName(obj.Product_TypeName);
                    product.Product_TypeId(obj.Product_TypeId);
                    product.Product_Type('Accessories');
                    break;
            }
            console.log('product:', ko.toJSON(product));
            self.Temp_Selling_Cart.push(product);
            console.log(ko.toJSON(self.Temp_Selling_Cart()));
        }
        self.Search_Key_For_Sell('');
        self.GameList_ForSell.removeAll();
        self.ConsoleList_ForSell.removeAll();
        self.AccessoriesList_ForSell.removeAll();        
    };

    self.Remove_Item_From_ShopingCart = function (obj) {
        //console.log('Remove_Item_From_ShopingCart', obj);
        self.Set_Cart_Status('false');
        self.Cart.ShopingCart.remove(obj);
        localStorage.setObject(self.ShopingCartKey, self.Cart);
        self.Save_Cart_details_in_Db();
    };

    self.Remove_Item_From_SellingCart = function (obj) {
        //console.log('Remove_Item_From_ShopingCart', obj);
        self.Set_SellingCart_Status('false');
        self.Cart2.SellingCart.remove(obj);
        localStorage.setObject(self.SellingCartKey, self.Cart2);
        self.Save_SellingCart_details_in_Db();
    };

    self.Update_Quantity = function () {
        //console.log('Update_Quantity');
        localStorage.setObject(self.ShopingCartKey, self.Cart);
        var cart = localStorage.getObject(self.ShopingCartKey);
        //console.log('Cart', cart);
        self.Set_Cart_Status('false');
        self.Update_Total_Price();
    };

    self.ShopingCartViewModel = function () {
        var x = this;
        x.Product_Id = ko.observable(0);
        x.Product_Name = ko.observable('');
        x.Product_Type = ko.observable('');
        x.Product_TypeId = ko.observable(0);
        x.Product_TypeName = ko.observable('');
        x.Quantity = ko.observable(0).extend({ notify: 'always' });
        x.Price = ko.observable(0);        
        x.ImageUrl = ko.observable('');
        x.PageUrl = ko.observable('');
        x.CartType = "Shopping";
        x.SKU = ko.observable('');
        x.Pre_Owned_Oprions = [{ value: false, name: 'New' }, { value: true, name: 'Pre Owned' }];
        x.TotalPrice = ko.computed(function () {
            var price = price = parseFloat(x.Price()) * parseInt(x.Quantity());
            return parseFloat(price).toFixed(2);
        }, x);
        
    };

    self.SellingCartViewModel = function () {
        var x = this;
        x.Product_Id = ko.observable(0);
        x.Condition = ko.observable(-1);
        x.BoxValue = ko.observable(-1);
        x.ManualValue = ko.observable(-1);
        x.Product_Name = ko.observable('');
        x.Product_Type = ko.observable('');
        x.Product_TypeId = ko.observable(0);
        x.Product_TypeName = ko.observable('');
        x.Quantity = ko.observable(1);        
        x.Price = ko.observable(0);
        x.ImageUrl = ko.observable('');
        x.PageUrl = ko.observable('');
        x.CartType = "Selling";
        x.SKU = ko.observable('');

        x.IsProcessing = ko.observable(false);
        x.IsItemProcessed = ko.observable(false);
        x.IsAllConditionSelected = ko.computed(function () {
            var cnd = x.Condition() || -1;
            var box = x.BoxValue() || -1;
            var manual = x.ManualValue() || -1;
            console.log('cnd:', cnd, ' box:', box, ' manual:', manual);
            return parseInt(cnd) !== -1 && parseInt(box) !== -1 && parseInt(manual) !== -1;
        }, x);

        x.TotalPrice = ko.computed(function () {
            var price = parseFloat(x.Price()) * parseInt(x.Quantity());            
            return parseFloat(price).toFixed(2);
        }, x);

        x.Deduction = ko.computed(function () {
            var price = parseFloat(x.Price()) * parseInt(x.Quantity());
            var _Condition = x.Condition() || 0;
            var _Manual = x.ManualValue() || 0;
            var _Box = x.BoxValue() || 0;
            var totalLess = parseInt(_Condition) + parseInt(_Box) + parseInt(_Manual);
            var lessPrice = (parseFloat(price) * totalLess) / 100;
            //lessPrice = lessPrice.toFixed(2);
            var UpdateCartPrice = parseFloat(price) - parseFloat(lessPrice);
            //console.log('price:', price, '_Condition:', _Condition, '_Box:', _Box, 'lessPrice:', lessPrice, 'totalLess:', totalLess, 'UpdateCartPrice:', UpdateCartPrice);
            return parseFloat(lessPrice).toFixed(2);
        }, x);

        x.Sub_Total = ko.computed(function () {
            var price = parseFloat(x.Price()) * parseInt(x.Quantity());
            var _Condition = x.Condition() || 0;
            var _Manual = x.ManualValue() || 0;
            var _Box = x.BoxValue() || 0;
            var totalLess = parseInt(_Condition) + parseInt(_Box) + parseInt(_Manual);
            var lessPrice = (parseFloat(price) * totalLess) / 100;
            //lessPrice = lessPrice.toFixed(2);
            var UpdateCartPrice = parseFloat(price) - parseFloat(lessPrice);
            //console.log('price:', price, '_Condition:', _Condition, '_Box:', _Box, 'lessPrice:', lessPrice, 'totalLess:', totalLess, 'UpdateCartPrice:', UpdateCartPrice);
            return parseFloat(UpdateCartPrice).toFixed(2);
        }, x);

        
        
    };

    self.Can_Display_Sell_Credits = ko.computed(function () {
        var list = self.Cart2.SellingCart();
        var carts = ko.utils.arrayFilter(list, function (o) {
            return o.IsItemProcessed() && o.IsAllConditionSelected();
        });
        console.log('Can_Display_Sell_Credits', carts.length);
        return carts.length;
    }, self);

    self.Sell_This_Product = function (obj) {
        console.log('Sell_This_Product', ko.toJSON(obj));
        var _product = ko.utils.arrayFirst(self.Temp_Selling_Cart(), function (o) {
            return o.SKU() === obj.SKU();
        });
        _product.IsItemProcessed(true);
        console.log('_product', _product);
        self.Set_SellingCart_Status('false');
        self.Cart2.SellingCart.push(_product);
        localStorage.setObject(self.SellingCartKey, self.Cart2);
        self.Temp_Selling_Cart.removeAll();
    };

    self.Check_Local_Storage_And_Update_Cart = function () {
        var cart = localStorage.getObject(self.ShopingCartKey);
        console.log('Check_Local_Storage_And_Update_ShoppingCart', cart);
        if (cart !== null) {
            self.Cart.Id(cart.Id);
            self.Cart.ShopingCart.removeAll();
            $.each(cart.ShopingCart, function (i, o) {
                var product = new self.ShopingCartViewModel();                
                product.Product_TypeName(o.Product_TypeName);                
                product.Product_TypeId(o.Product_TypeId);
                product.Product_Type(o.Product_Type);
                product.Product_Name(o.Product_Name);
                product.Product_Id(o.Product_Id);                
                product.Quantity(o.Quantity);
                product.ImageUrl(o.ImageUrl);
                product.PageUrl(o.PageUrl);
                product.Price(o.Price);
                product.SKU(o.SKU);
                self.Cart.ShopingCart.push(product);
            });
        }

        var cart2 = localStorage.getObject(self.SellingCartKey);
        console.log('Check_Local_Storage_And_Update_SellingCart', cart2);
        if (cart2 !== null) {
            self.Cart2.Id(cart2.Id);
            self.Cart2.SellingCart.removeAll();
            $.each(cart2.SellingCart, function (i, o) {
                var product = new self.SellingCartViewModel();
                product.Product_TypeName(o.Product_TypeName);
                product.Product_TypeId(o.Product_TypeId);
                product.Product_Type(o.Product_Type);
                product.Product_Name(o.Product_Name);
                product.Product_Id(o.Product_Id);
                product.Quantity(o.Quantity);
                product.ImageUrl(o.ImageUrl);
                product.PageUrl(o.PageUrl);
                product.BoxValue(o.BoxValue);
                product.Condition(o.Condition);
                product.ManualValue(o.ManualValue);
                product.Price(o.Price);
                product.SKU(o.SKU);
                product.IsItemProcessed(o.IsItemProcessed);
                self.Cart2.SellingCart.push(product);
            });
        }
    };

    self.Save_Cart_details_in_Db = function () {
        //console.log('Save_Cart_details_in_Db');
        self.IsUpdating_Cart(true);
        var status = self.Get_Cart_Status();
        //console.log('status', status);
        if (self.Cart.ShopingCart().length > 0) {
            var _data = {
                Status: 'Ready',
                CartType: 'Shopping',
                Id: self.Cart.Id(),
                UserId: self.Cart.UserId(),                
                Total_Price: self.Total_Price(),
                Tax_Rate: self.Tax_Rate(),                
                Total_Sum: self.Sub_Total(),
                Tax_Amount: self.Tax_Amount(),
                ShippingCharge: self.ShippingCharge(),
                Total_Item: self.Cart.ShopingCart().length,
                ShopingCart: self.Cart.ShopingCart()
            };
            //console.log('_data', _data);
           $.ajax({
                type: 'POST', dataType: "json", url: '/shopping-cart/update',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON(_data),
                success: function (data) {
                    console.log('data', data);
                    self.IsUpdating_Cart(false);
                    self.Set_Cart_Status('true');
                    self.Cart.Id(data.Id);
                    self.Cart.ShopingCart.removeAll();
                    $.each(data.ShopingCart, function (i, o) {
                        var product = new self.ShopingCartViewModel();
                        product.Product_TypeName(o.Product_TypeName);
                        product.Product_TypeId(o.Product_TypeId);
                        product.Product_Type(o.Product_Type);
                        product.Product_Name(o.Product_Name);
                        product.Product_Id(o.Product_Id);
                        product.Quantity(o.Quantity);
                        product.ImageUrl(o.ImageUrl);
                        product.PageUrl(o.PageUrl);
                        product.Price(o.Price);
                        product.SKU(o.SKU);
                        self.Cart.ShopingCart.push(product);
                    });
                    localStorage.setObject(self.ShopingCartKey, self.Cart);
                },
                error: function (result) { console.log(result); }
            });
        }
    };

    self.Save_SellingCart_details_in_Db = function () {
        console.log('Save_SellingCart_details_in_Db');
        self.IsUpdating_Cart(true);
        var status = self.Get_SellingCart_Status();
        console.log('status', status);
        if (self.Cart2.SellingCart().length > 0) {
            var _data = {
                Status: 'Ready',
                CartType: 'Selling',
                Id: self.Cart2.Id(),
                Tax_Rate: self.Tax_Rate(),
                UserId: self.Cart2.UserId(),                
                Total_Price: self.Total_Price_Sell(),
                Total_Sum: self.Sub_Total_Sell(),
                Tax_Amount: self.Tax_Amount_Sell(),
                ShippingCharge: self.ShippingCharge(),
                Total_Deduction: self.Total_Deduction(),
                Total_Item: self.Cart2.SellingCart().length,
                SellingCart: self.Cart2.SellingCart()
            };
            console.log('_data', ko.toJSON(_data)); //return;
            $.ajax({
                type: 'POST', dataType: "json", url: '/selling-cart/update',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON(_data),
                success: function (data) {
                    console.log('data', data);
                    self.IsUpdating_Cart(false);
                    self.Set_SellingCart_Status('true');
                    self.Cart2.Id(data.Id);
                    self.Cart2.SellingCart.removeAll();
                    $.each(data.SellingCart, function (i, o) {
                        var product = new self.SellingCartViewModel();
                        product.Product_TypeName(o.Product_TypeName);
                        product.Product_TypeId(o.Product_TypeId);
                        product.Product_Type(o.Product_Type);
                        product.Product_Name(o.Product_Name);
                        product.Product_Id(o.Product_Id);
                        product.Quantity(o.Quantity);
                        product.ImageUrl(o.ImageUrl);
                        product.PageUrl(o.PageUrl);
                        product.Price(o.Price);
                        product.SKU(o.SKU);
                        product.BoxValue(o.BoxValue);
                        product.Condition(o.Condition);
                        product.ManualValue(o.ManualValue);
                        self.Cart2.SellingCart.push(product);
                    });
                    localStorage.setObject(self.SellingCartKey, self.Cart2);
                },
                error: function (result) { console.log(result); }
            });
        }
    };

    self.Check_out_SellingCart = function () {
        console.log('Check_out_SellingCart');
        var _product = ko.utils.arrayFirst(self.Cart2.SellingCart(), function (o) {
            return parseInt(o.BoxValue()) === -1 || parseInt(o.ManualValue()) === -1 || parseInt(o.Condition()) === -1;
        });
        if (_product !== null) {
            self.Cart2.SellingCart.remove(_product);
        }
        if (self.Cart2.SellingCart().length > 0) {
            var _data = {
                Status: 'Ready',
                CartType: 'Selling',
                Id: self.Cart2.Id(),
                Tax_Rate: self.Tax_Rate(),
                UserId: self.Cart2.UserId(),
                Total_Price: self.Total_Price_Sell(),
                Total_Sum: self.Sub_Total_Sell(),
                Tax_Amount: self.Tax_Amount_Sell(),
                ShippingCharge: self.ShippingCharge(),
                Total_Deduction: self.Total_Deduction(),
                Total_Item: self.Cart2.SellingCart().length,
                SellingCart: self.Cart2.SellingCart()
            };
            console.log('_data', ko.toJSON(_data)); //return;
            $.ajax({
                type: 'POST', dataType: "json", url: '/selling-cart/sell',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON(_data),
                success: function (result) {
                    console.log('data', result);
                    self.Cart2.Id(result);
                    localStorage.setObject(self.SellingCartKey, self.Cart2);
                    window.location.href = "/check-out-1/" + result;
                },
                error: function (result) { console.log(result); }
            });
        }
    };

    self.Check_out_Shopping_Cart = function () {
        console.log('Check_out_Shopping_Cart');
        
        if (self.Cart.ShopingCart().length > 0) {
            var _data = {
                Status: 'Ready',
                CartType: 'Shopping',
                Id: self.Cart.Id(),
                Tax_Rate: self.Tax_Rate(),
                UserId: self.Cart.UserId(),
                PromoCode: self.PromoCode(),
                Total_Sum: self.Sub_Total(),
                Tax_Amount: self.Tax_Amount(),
                CreditUsed: self.CreditUsed(),
                Total_Price: self.Total_Price(),
                ShopingCart: self.Cart.ShopingCart(),
                ShippingCharge: self.ShippingCharge(),
                Total_Item: self.Cart.ShopingCart().length,
            };
            console.log('_data', _data);
            self.IsUpdating_Cart(true);
            $.when(
                $.ajax({
                    type: 'POST', dataType: "json",
                    url: '/shopping-cart/update-total-price',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON(_data)
                })
            ).then(function (data) {
                window.location.href = "/check-out-1/" + self.Cart.Id();
            });

        }
    };

    self.Get_Shopping_Cart_Details = function () {
        //console.log('Get_Shopping_Cart_Details', self.Cart.UserId());
        self.IsUpdating_Cart(true);
        $.ajax({
            type: 'GET', dataType: "json",
            url: '/shopping-cart/' + self.Cart.UserId(),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                //console.log('data', data);
                self.IsUpdating_Cart(false);
                if (data != null) {
                    self.Cart.Id(data.Id);
                    if (data.ShopingCart != null) {
                        self.Cart.ShopingCart.removeAll();
                        $.each(data.ShopingCart, function (i, o) {
                            var product = new self.ShopingCartViewModel();
                            product.Product_TypeName(o.Product_TypeName);
                            product.Product_TypeId(o.Product_TypeId);
                            product.Product_Type(o.Product_Type);
                            product.Product_Name(o.Product_Name);
                            product.Product_Id(o.Product_Id);
                            product.Quantity(o.Quantity);
                            product.ImageUrl(o.ImageUrl);
                            product.PageUrl(o.PageUrl);
                            product.Price(o.Price);
                            product.SKU(o.SKU);
                            self.Cart.ShopingCart.push(product);
                        });
                    }                    
                    localStorage.setObject(self.ShopingCartKey, self.Cart);
                }                
            },
            error: function (result) { console.log(result); }
        });
    };

    self.Get_Selling_Cart_Details = function () {
        console.log('Get_Selling_Cart_Details', self.Cart.UserId());
        self.IsUpdating_Cart(true);
        $.ajax({
            type: 'GET', dataType: "json",
            url: '/selling-cart/' + self.Cart2.UserId(),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                console.log('data', data);
                self.IsUpdating_Cart(false);
                if (data != null) {
                    self.Cart2.Id(data.Id);
                    if (data.SellingCart != null) {
                        self.Cart2.SellingCart.removeAll();
                        $.each(data.SellingCart, function (i, o) {
                            var product = new self.SellingCartViewModel();
                            product.Product_TypeName(o.Product_TypeName);
                            product.Product_TypeId(o.Product_TypeId);
                            product.Product_Type(o.Product_Type);
                            product.Product_Name(o.Product_Name);
                            product.Product_Id(o.Product_Id);
                            product.Quantity(o.Quantity);
                            product.ImageUrl(o.ImageUrl);
                            product.PageUrl(o.PageUrl);
                            product.Price(o.Price);
                            product.SKU(o.SKU);
                            product.BoxValue(o.BoxValue);
                            product.Condition(o.Condition);
                            product.ManualValue(o.ManualValue);
                            self.Cart2.SellingCart.push(product);
                        });
                    }
                    localStorage.setObject(self.SellingCartKey, self.Cart2);
                }
            },
            error: function (result) { console.log(result); }
        });
    };

    self.OnClick_LogOff = function () {
        localStorage.clear();
        document.getElementById("logoutForm").submit();
    };

    self.Manage_Shopping_Cart = function () {
        if (self.Cart.ShopingCart().length > 0) {
            var _data = {
                Status: 'Ready',
                CartType: 'Shopping',
                Id: self.Cart.Id(),
                UserId: self.Cart.UserId(),                
                Total_Price: self.Total_Price(),
                Tax_Rate: self.Tax_Rate(),
                Total_Sum: self.Sub_Total(),
                Tax_Amount: self.Tax_Amount(),
                ShippingCharge: self.ShippingCharge(),
                Total_Item: self.Cart.ShopingCart().length,
                ShopingCart: self.Cart.ShopingCart()
            };
            //console.log('_data', _data);

            $.when(
                $.ajax({
                    type: 'POST', dataType: "json",
                    url: '/shopping-cart/update',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON(_data)
                })
            ).then(function (data) {
                //console.log('Manage_Shopping_Cart: data', data);
                if (self.Cart.UserId() > 0) { self.Get_Shopping_Cart_Details(); }
                else {
                    self.IsUpdating_Cart(false);
                    self.Set_Cart_Status('true');
                    self.Cart.Id(data.Id);
                    self.Cart.ShopingCart.removeAll();
                    $.each(data.ShopingCart, function (i, o) {
                        var product = new self.ShopingCartViewModel();
                        product.Product_TypeName(o.Product_TypeName);
                        product.Product_TypeId(o.Product_TypeId);
                        product.Product_Type(o.Product_Type);
                        product.Product_Name(o.Product_Name);
                        product.Product_Id(o.Product_Id);
                        product.Quantity(o.Quantity);
                        product.ImageUrl(o.ImageUrl);
                        product.PageUrl(o.PageUrl);
                        product.Price(o.Price);
                        product.SKU(o.SKU);
                        self.Cart.ShopingCart.push(product);
                    });
                    localStorage.setObject(self.ShopingCartKey, self.Cart);
                }
            });
        }
    };

    self.Manage_Selling_Cart = function () {
        if (self.Cart2.SellingCart().length > 0) {
            var _data = {
                Status: 'Ready',
                CartType: 'Selling',
                Id: self.Cart2.Id(),
                Tax_Rate: self.Tax_Rate(),
                UserId: self.Cart2.UserId(),                
                Total_Price: self.Total_Price_Sell(),
                Total_Sum: self.Sub_Total_Sell(),
                Total_Deduction: self.Total_Deduction(),
                Tax_Amount: self.Tax_Amount_Sell(),
                ShippingCharge: self.ShippingCharge(),
                Total_Item: self.Cart2.SellingCart().length,
                SellingCart: self.Cart2.SellingCart()
            };
            //console.log('_data', _data);

            $.when(
                $.ajax({
                    type: 'POST', dataType: "json",
                    url: '/selling-cart/update',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON(_data)
                })
            ).then(function (data) {
                //console.log('Manage_Shopping_Cart: data', data);
                if (self.Cart2.UserId() > 0) { self.Get_Shopping_Cart_Details(); }
                else {
                    self.IsUpdating_Cart(false);
                    self.Set_SellingCart_Status('true');
                    self.Cart2.Id(data.Id);
                    self.Cart2.SellingCart.removeAll();
                    $.each(data.SellingCart, function (i, o) {
                        var product = new self.SellingCartViewModel();
                        product.Product_TypeName(o.Product_TypeName);
                        product.Product_TypeId(o.Product_TypeId);
                        product.Product_Type(o.Product_Type);
                        product.Product_Name(o.Product_Name);
                        product.Product_Id(o.Product_Id);
                        product.Quantity(o.Quantity);
                        product.ImageUrl(o.ImageUrl);
                        product.PageUrl(o.PageUrl);
                        product.Price(o.Price);
                        product.SKU(o.SKU);
                        product.BoxValue(o.BoxValue);
                        product.Condition(o.Condition);
                        product.ManualValue(o.ManualValue);
                        self.Cart2.SellingCart.push(product);
                    });
                    localStorage.setObject(self.SellingCartKey, self.Cart2);
                }
            });
        }
    };

    self.Update_Total_Price = function () {
        if (self.Cart.ShopingCart().length > 0) {
            var _data = {
                Status: 'Ready',
                CartType: 'Shopping',
                Id: self.Cart.Id(),
                UserId: self.Cart.UserId(),                
                Total_Price: self.Total_Price(),
                Tax_Rate: self.Tax_Rate(),
                Total_Sum: self.Sub_Total(),
                Tax_Amount: self.Tax_Amount(),
                ShippingCharge: self.ShippingCharge(),
                Total_Item: self.Cart.ShopingCart().length,
                ShopingCart: self.Cart.ShopingCart()
            };
            //console.log('_data', _data);

            $.when(
                $.ajax({
                    type: 'POST', dataType: "json",
                    url: '/shopping-cart/update-total-price',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON(_data)
                })
            ).then(function (data) {
                //console.log('Update_Total_Price: data', data);                
            });
            
        }
    };

    self.Update_Total_Price_Sell = function () {
        if (self.Cart2.SellingCart().length > 0) {
            var _data = {
                Status: 'Ready',
                CartType: 'Selling',
                Id: self.Cart2.Id(),
                Tax_Rate: self.Tax_Rate(),
                UserId: self.Cart2.UserId(),                
                Total_Price: self.Total_Price_Sell(),                
                Total_Sum: self.Sub_Total_Sell(),
                Tax_Amount: self.Tax_Amount_Sell(),
                Total_Deduction: self.Total_Deduction(),
                Total_Item: self.Cart2.SellingCart().length,
                ShippingCharge: self.ShippingCharge(),
                SellingCart: self.Cart2.SellingCart()
            };
            //console.log('_data', _data);

            $.when(
                $.ajax({
                    type: 'POST', dataType: "json",
                    url: '/selling-cart/update-total-price',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON(_data)
                })
            ).then(function (data) {
                //console.log('Update_Total_Price: data', data);                
            });

        }
    };

    self.Add_To_WishList = function (product_id, product_type_id, wishType, price, sku) {
        var _data = { WishType: wishType, Product_Id: product_id, ProductType_Id: product_type_id, Price: price, SKU: sku };
        $.ajax({
            type: 'POST', dataType: "json", url: '/add-to-wishlist',
            contentType: "application/json; charset=utf-8",
            data: ko.toJSON(_data),
            success: function (obj) {
                if (Boolean(obj)) {
                    alert("This product added in your wishlist.");
                } else {
                    alert("Already added in your wishlist.");
                }
            }
        });
    };
        
};

function create_cookies(target) {
    var value = "; " + document.cookie;
    var parts = value.includes("WebsiteCookies=true");
    if (!parts) {
        document.cookie = "WebsiteCookies=true";
    }
    document.getElementById(target).style.display = 'none';
}
function hide_cookies(target) {
    document.getElementById(target).style.display = 'none';
}

function onBegin(e) {    
        $('.submitBtn').val('Processing...');
        $('.submitBtn').prop('disabled', true);
};

function onComplete() {
    $('.submitBtn').val('');
    $('.submitBtn').prop('disabled', false);
}


$(document).ready(function () {
    //localStorage.clear();
    _app = new MainAppViewModel();
    ko.applyBindings(_app);
    var userId = $('#game-store-user-id').val() || 0;
    userId = parseInt(userId);
    _app.Cart.UserId(userId);
    _app.Cart2.UserId(userId);
    _app.Check_Local_Storage_And_Update_Cart();
    if (userId > 0) {
        _app.Get_Selling_Cart_Details();
        _app.Get_Shopping_Cart_Details();
    } else {
        _app.Manage_Shopping_Cart();
        _app.Manage_Selling_Cart();
    }
    
    
   

    $('.main-menu').mobileMegaMenu({
        changeToggleText: true,
        enableWidgetRegion: true,
        prependCloseButton: true,
        stayOnActive: true,
        toogleTextOnClose: 'X',
        menuToggle: 'main-menu-toggle'
    });

    
   
   var $animation_elements = $('.animation-element');
    var $window = $(window);

    function check_if_in_view() {
        var window_height = $window.height();
        var window_top_position = $window.scrollTop();
        var window_bottom_position = (window_top_position + window_height);
        $.each($animation_elements, function() {
            var $element = $(this);
            var element_height = $element.outerHeight();
            var element_top_position = $element.offset().top;
            var element_bottom_position = (element_top_position + element_height);
            if ((element_bottom_position >= window_top_position) && (element_top_position <= window_bottom_position)) {
                $element.addClass('in-view');
            } else {
                $element.removeClass('in-view');
            }
        });
    }
    $window.on('scroll resize', check_if_in_view);
    $window.trigger('scroll');

    $('#imgs').imageScroll({
        orientation: "left",
        speed: 600,
        interval: 1000,
        hoverPause: true,
    });

    $(window).scroll(function () {
        var scroll = $(window).scrollTop();
        if (scroll >= 40) {
            $("body").addClass("smaller");
        } else {
            $("body").removeClass("smaller");
        }
    });


    var delay = (function () {
        var timer = 0;
        return function (callback, ms) {
            clearTimeout(timer);
            timer = setTimeout(callback, ms);
        };
    })();

    //change paste input
    $('#search-box').on('change input', function (e) {
        delay(function () {
            console.log('Event: ', e.type);
            var query = e.target.value || '';
            console.log(query);
            if (query.length > 0) {
                $('.search-processing-icon').show();
                $('.search-remove-icon').hide();
                $.ajax({
                    type: 'POST', dataType: "html",
                    url: '/search', data: JSON.stringify({ Query: query }),
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $('#search-result').html(data);
                        $('.search-processing-icon').hide();
                        $('.search-remove-icon').show();
                    }
                });
            } else { $('#search-result').html(''); }
        }, 500);
    });

    $('.search-remove-icon').on('click', function (e) {
        $('.search-remove-icon').hide();
        $('#search-box').val('');
    });
    

});


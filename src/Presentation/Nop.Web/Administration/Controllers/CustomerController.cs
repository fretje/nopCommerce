﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Mvc;
using Nop.Admin.Extensions;
using Nop.Admin.Models.Common;
using Nop.Admin.Models.Customers;
using Nop.Admin.Models.ShoppingCart;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Tax;
using Nop.Services.Affiliates;
using Nop.Services.Authentication.External;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.ExportImport;
using Nop.Services.Forums;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Controllers
{
    public partial class CustomerController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ICustomerReportService _customerReportService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly TaxSettings _taxSettings;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IAddressService _addressService;
        private readonly CustomerSettings _customerSettings;
        private readonly ITaxService _taxService;
        private readonly IWorkContext _workContext;
        private readonly IVendorService _vendorService;
        private readonly IStoreContext _storeContext;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IOrderService _orderService;
        private readonly IExportManager _exportManager;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IProductAttributeFormatter _productAttributeFormatter;
        private readonly IPermissionService _permissionService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IEmailAccountService _emailAccountService;
        private readonly ForumSettings _forumSettings;
        private readonly IForumService _forumService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly AddressSettings _addressSettings;
        private readonly IStoreService _storeService;
        private readonly ICustomerAttributeParser _customerAttributeParser;
        private readonly ICustomerAttributeService _customerAttributeService;
        private readonly IAddressAttributeParser _addressAttributeParser;
        private readonly IAddressAttributeService _addressAttributeService;
        private readonly IAddressAttributeFormatter _addressAttributeFormatter;
        private readonly IAffiliateService _affiliateService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IRewardPointService _rewardPointService;

        #endregion

        #region Constructors

        public CustomerController(ICustomerService customerService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IGenericAttributeService genericAttributeService,
            ICustomerRegistrationService customerRegistrationService,
            ICustomerReportService customerReportService, 
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService, 
            DateTimeSettings dateTimeSettings,
            TaxSettings taxSettings, 
            RewardPointsSettings rewardPointsSettings,
            ICountryService countryService, 
            IStateProvinceService stateProvinceService, 
            IAddressService addressService,
            CustomerSettings customerSettings,
            ITaxService taxService, 
            IWorkContext workContext,
            IVendorService vendorService,
            IStoreContext storeContext,
            IPriceFormatter priceFormatter,
            IOrderService orderService, 
            IExportManager exportManager,
            ICustomerActivityService customerActivityService,
            IBackInStockSubscriptionService backInStockSubscriptionService,
            IPriceCalculationService priceCalculationService,
            IProductAttributeFormatter productAttributeFormatter,
            IPermissionService permissionService, 
            IQueuedEmailService queuedEmailService,
            EmailAccountSettings emailAccountSettings,
            IEmailAccountService emailAccountService, 
            ForumSettings forumSettings,
            IForumService forumService, 
            IOpenAuthenticationService openAuthenticationService,
            AddressSettings addressSettings,
            IStoreService storeService,
            ICustomerAttributeParser customerAttributeParser,
            ICustomerAttributeService customerAttributeService,
            IAddressAttributeParser addressAttributeParser,
            IAddressAttributeService addressAttributeService,
            IAddressAttributeFormatter addressAttributeFormatter,
            IAffiliateService affiliateService,
            IWorkflowMessageService workflowMessageService,
            IRewardPointService rewardPointService)
        {
            this._customerService = customerService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._genericAttributeService = genericAttributeService;
            this._customerRegistrationService = customerRegistrationService;
            this._customerReportService = customerReportService;
            this._dateTimeHelper = dateTimeHelper;
            this._localizationService = localizationService;
            this._dateTimeSettings = dateTimeSettings;
            this._taxSettings = taxSettings;
            this._rewardPointsSettings = rewardPointsSettings;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._addressService = addressService;
            this._customerSettings = customerSettings;
            this._taxService = taxService;
            this._workContext = workContext;
            this._vendorService = vendorService;
            this._storeContext = storeContext;
            this._priceFormatter = priceFormatter;
            this._orderService = orderService;
            this._exportManager = exportManager;
            this._customerActivityService = customerActivityService;
            this._backInStockSubscriptionService = backInStockSubscriptionService;
            this._priceCalculationService = priceCalculationService;
            this._productAttributeFormatter = productAttributeFormatter;
            this._permissionService = permissionService;
            this._queuedEmailService = queuedEmailService;
            this._emailAccountSettings = emailAccountSettings;
            this._emailAccountService = emailAccountService;
            this._forumSettings = forumSettings;
            this._forumService = forumService;
            this._openAuthenticationService = openAuthenticationService;
            this._addressSettings = addressSettings;
            this._storeService = storeService;
            this._customerAttributeParser = customerAttributeParser;
            this._customerAttributeService = customerAttributeService;
            this._addressAttributeParser = addressAttributeParser;
            this._addressAttributeService = addressAttributeService;
            this._addressAttributeFormatter = addressAttributeFormatter;
            this._affiliateService = affiliateService;
            this._workflowMessageService = workflowMessageService;
            this._rewardPointService = rewardPointService;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected virtual string GetCustomerRolesNames(IList<CustomerRole> customerRoles, string separator = ",")
        {
            var sb = new StringBuilder();
            for (int i = 0; i < customerRoles.Count; i++)
            {
                sb.Append(customerRoles[i].Name);
                if (i != customerRoles.Count - 1)
                {
                    sb.Append(separator);
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

        [NonAction]
        protected virtual IList<RegisteredCustomerReportLineModel> GetReportRegisteredCustomersModel()
        {
            var report = new List<RegisteredCustomerReportLineModel>();
            report.Add(new RegisteredCustomerReportLineModel
            {
                Period = _localizationService.GetResource("Admin.Customers.Reports.RegisteredCustomers.Fields.Period.7days"),
                Customers = _customerReportService.GetRegisteredCustomersReport(7)
            });

            report.Add(new RegisteredCustomerReportLineModel
            {
                Period = _localizationService.GetResource("Admin.Customers.Reports.RegisteredCustomers.Fields.Period.14days"),
                Customers = _customerReportService.GetRegisteredCustomersReport(14)
            });
            report.Add(new RegisteredCustomerReportLineModel
            {
                Period = _localizationService.GetResource("Admin.Customers.Reports.RegisteredCustomers.Fields.Period.month"),
                Customers = _customerReportService.GetRegisteredCustomersReport(30)
            });
            report.Add(new RegisteredCustomerReportLineModel
            {
                Period = _localizationService.GetResource("Admin.Customers.Reports.RegisteredCustomers.Fields.Period.year"),
                Customers = _customerReportService.GetRegisteredCustomersReport(365)
            });

            return report;
        }

        [NonAction]
        protected virtual IList<CustomerModel.AssociatedExternalAuthModel> GetAssociatedExternalAuthRecords(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var result = new List<CustomerModel.AssociatedExternalAuthModel>();
            foreach (var record in _openAuthenticationService.GetExternalIdentifiersFor(customer))
            {
                var method = _openAuthenticationService.LoadExternalAuthenticationMethodBySystemName(record.ProviderSystemName);
                if (method == null)
                    continue;

                result.Add(new CustomerModel.AssociatedExternalAuthModel
                {
                    Id = record.Id,
                    Email = record.Email,
                    ExternalIdentifier = record.ExternalIdentifier,
                    AuthMethodName = method.PluginDescriptor.FriendlyName
                });
            }

            return result;
        }

        [NonAction]
        protected virtual CustomerModel PrepareCustomerModelForList(Customer customer)
        {
            return new CustomerModel
            {
                Id = customer.Id,
                Email = customer.IsRegistered() ? customer.Email : _localizationService.GetResource("Admin.Customers.Guest"),
                Username = customer.Username,
                FullName = customer.GetFullName(),
                Company = customer.GetAttribute<string>(SystemCustomerAttributeNames.Company),
                Phone = customer.GetAttribute<string>(SystemCustomerAttributeNames.Phone),
                ZipPostalCode = customer.GetAttribute<string>(SystemCustomerAttributeNames.ZipPostalCode),
                CustomerRoleNames = GetCustomerRolesNames(customer.CustomerRoles.ToList()),
                Active = customer.Active,
                CreatedOn = _dateTimeHelper.ConvertToUserTime(customer.CreatedOnUtc, DateTimeKind.Utc),
                LastActivityDate = _dateTimeHelper.ConvertToUserTime(customer.LastActivityDateUtc, DateTimeKind.Utc),
            };
        }

        [NonAction]
        protected virtual string ValidateCustomerRoles(IList<CustomerRole> customerRoles)
        {
            if (customerRoles == null)
                throw new ArgumentNullException("customerRoles");

            //ensure a customer is not added to both 'Guests' and 'Registered' customer roles
            //ensure that a customer is in at least one required role ('Guests' and 'Registered')
            bool isInGuestsRole = customerRoles.FirstOrDefault(cr => cr.SystemName == SystemCustomerRoleNames.Guests) != null;
            bool isInRegisteredRole = customerRoles.FirstOrDefault(cr => cr.SystemName == SystemCustomerRoleNames.Registered) != null;
            if (isInGuestsRole && isInRegisteredRole)
                return "The customer cannot be in both 'Guests' and 'Registered' customer roles";
            if (!isInGuestsRole && !isInRegisteredRole)
                return "Add the customer to 'Guests' or 'Registered' customer role";

            //no errors
            return "";
        }

        [NonAction]
        protected virtual void PrepareVendorsModel(CustomerModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            model.AvailableVendors.Add(new SelectListItem
            {
                Text = _localizationService.GetResource("Admin.Customers.Customers.Fields.Vendor.None"),
                Value = "0"
            });
            var vendors = _vendorService.GetAllVendors(showHidden: true);
            foreach (var vendor in vendors)
            {
                model.AvailableVendors.Add(new SelectListItem
                {
                    Text = vendor.Name,
                    Value = vendor.Id.ToString()
                });
            }
        }

        [NonAction]
        protected virtual void PrepareCustomerAttributeModel(CustomerModel model, Customer customer)
        {
            var customerAttributes = _customerAttributeService.GetAllCustomerAttributes();
            foreach (var attribute in customerAttributes)
            {
                var attributeModel = new CustomerModel.CustomerAttributeModel
                {
                    Id = attribute.Id,
                    Name = attribute.Name,
                    IsRequired = attribute.IsRequired,
                    AttributeControlType = attribute.AttributeControlType,
                };

                if (attribute.ShouldHaveValues())
                {
                    //values
                    var attributeValues = _customerAttributeService.GetCustomerAttributeValues(attribute.Id);
                    foreach (var attributeValue in attributeValues)
                    {
                        var attributeValueModel = new CustomerModel.CustomerAttributeValueModel
                        {
                            Id = attributeValue.Id,
                            Name = attributeValue.Name,
                            IsPreSelected = attributeValue.IsPreSelected
                        };
                        attributeModel.Values.Add(attributeValueModel);
                    }
                }


                //set already selected attributes
                if (customer != null)
                {
                    var selectedCustomerAttributes = customer.GetAttribute<string>(SystemCustomerAttributeNames.CustomCustomerAttributes, _genericAttributeService);
                    switch (attribute.AttributeControlType)
                    {
                        case AttributeControlType.DropdownList:
                        case AttributeControlType.RadioList:
                        case AttributeControlType.Checkboxes:
                        {
                            if (!String.IsNullOrEmpty(selectedCustomerAttributes))
                            {
                                //clear default selection
                                foreach (var item in attributeModel.Values)
                                    item.IsPreSelected = false;

                                //select new values
                                var selectedValues = _customerAttributeParser.ParseCustomerAttributeValues(selectedCustomerAttributes);
                                foreach (var attributeValue in selectedValues)
                                    foreach (var item in attributeModel.Values)
                                        if (attributeValue.Id == item.Id)
                                            item.IsPreSelected = true;
                            }
                        }
                            break;
                        case AttributeControlType.ReadonlyCheckboxes:
                        {
                            //do nothing
                            //values are already pre-set
                        }
                            break;
                        case AttributeControlType.TextBox:
                        case AttributeControlType.MultilineTextbox:
                        {
                            if (!String.IsNullOrEmpty(selectedCustomerAttributes))
                            {
                                var enteredText = _customerAttributeParser.ParseValues(selectedCustomerAttributes, attribute.Id);
                                if (enteredText.Count > 0)
                                    attributeModel.DefaultValue = enteredText[0];
                            }
                        }
                            break;
                        case AttributeControlType.Datepicker:
                        case AttributeControlType.ColorSquares:
                        case AttributeControlType.ImageSquares:
                        case AttributeControlType.FileUpload:
                        default:
                            //not supported attribute control types
                            break;
                    }
                }

                model.CustomerAttributes.Add(attributeModel);
            }
        }

        [NonAction]
        protected virtual string ParseCustomCustomerAttributes(Customer customer, FormCollection form)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (form == null)
                throw new ArgumentNullException("form");

            string attributesXml = "";
            var customerAttributes = _customerAttributeService.GetAllCustomerAttributes();
            foreach (var attribute in customerAttributes)
            {
                string controlId = string.Format("customer_attribute_{0}", attribute.Id);
                switch (attribute.AttributeControlType)
                {
                    case AttributeControlType.DropdownList:
                    case AttributeControlType.RadioList:
                        {
                            var ctrlAttributes = form[controlId];
                            if (!String.IsNullOrEmpty(ctrlAttributes))
                            {
                                int selectedAttributeId = int.Parse(ctrlAttributes);
                                if (selectedAttributeId > 0)
                                    attributesXml = _customerAttributeParser.AddCustomerAttribute(attributesXml,
                                        attribute, selectedAttributeId.ToString());
                            }
                        }
                        break;
                    case AttributeControlType.Checkboxes:
                        {
                            var cblAttributes = form[controlId];
                            if (!String.IsNullOrEmpty(cblAttributes))
                            {
                                foreach (var item in cblAttributes.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    int selectedAttributeId = int.Parse(item);
                                    if (selectedAttributeId > 0)
                                        attributesXml = _customerAttributeParser.AddCustomerAttribute(attributesXml,
                                            attribute, selectedAttributeId.ToString());
                                }
                            }
                        }
                        break;
                    case AttributeControlType.ReadonlyCheckboxes:
                        {
                            //load read-only (already server-side selected) values
                            var attributeValues = _customerAttributeService.GetCustomerAttributeValues(attribute.Id);
                            foreach (var selectedAttributeId in attributeValues
                                .Where(v => v.IsPreSelected)
                                .Select(v => v.Id)
                                .ToList())
                            {
                                attributesXml = _customerAttributeParser.AddCustomerAttribute(attributesXml,
                                            attribute, selectedAttributeId.ToString());
                            }
                        }
                        break;
                    case AttributeControlType.TextBox:
                    case AttributeControlType.MultilineTextbox:
                        {
                            var ctrlAttributes = form[controlId];
                            if (!String.IsNullOrEmpty(ctrlAttributes))
                            {
                                string enteredText = ctrlAttributes.Trim();
                                attributesXml = _customerAttributeParser.AddCustomerAttribute(attributesXml,
                                    attribute, enteredText);
                            }
                        }
                        break;
                    case AttributeControlType.Datepicker:
                    case AttributeControlType.ColorSquares:
                    case AttributeControlType.ImageSquares:
                    case AttributeControlType.FileUpload:
                    //not supported customer attributes
                    default:
                        break;
                }
            }

            return attributesXml;
        }

        [NonAction]
        protected virtual void PrepareCustomerModel(CustomerModel model, Customer customer, bool excludeProperties)
        {
            var allStores = _storeService.GetAllStores();
            if (customer != null)
            {
                model.Id = customer.Id;
                if (!excludeProperties)
                {
                    model.Email = customer.Email;
                    model.Username = customer.Username;
                    model.VendorId = customer.VendorId;
                    model.AdminComment = customer.AdminComment;
                    model.IsTaxExempt = customer.IsTaxExempt;
                    model.Active = customer.Active;

                    var affiliate = _affiliateService.GetAffiliateById(customer.AffiliateId);
                    if (affiliate != null)
                    {
                        model.AffiliateId = affiliate.Id;
                        model.AffiliateName = affiliate.GetFullName();
                    }

                    model.TimeZoneId = customer.GetAttribute<string>(SystemCustomerAttributeNames.TimeZoneId);
                    model.VatNumber = customer.GetAttribute<string>(SystemCustomerAttributeNames.VatNumber);
                    model.VatNumberStatusNote = ((VatNumberStatus)customer.GetAttribute<int>(SystemCustomerAttributeNames.VatNumberStatusId))
                        .GetLocalizedEnum(_localizationService, _workContext);
                    model.CreatedOn = _dateTimeHelper.ConvertToUserTime(customer.CreatedOnUtc, DateTimeKind.Utc);
                    model.LastActivityDate = _dateTimeHelper.ConvertToUserTime(customer.LastActivityDateUtc, DateTimeKind.Utc);
                    model.LastIpAddress = customer.LastIpAddress;
                    model.LastVisitedPage = customer.GetAttribute<string>(SystemCustomerAttributeNames.LastVisitedPage);

                    model.SelectedCustomerRoleIds = customer.CustomerRoles.Select(cr => cr.Id).ToArray();

                    //newsletter subscriptions
                    if (!String.IsNullOrEmpty(customer.Email))
                    {
                        var newsletterSubscriptionStoreIds = new List<int>();
                        foreach (var store in allStores)
                        {
                            var newsletterSubscription = _newsLetterSubscriptionService
                                .GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, store.Id);
                            if (newsletterSubscription != null)
                                newsletterSubscriptionStoreIds.Add(store.Id);
                            model.SelectedNewsletterSubscriptionStoreIds = newsletterSubscriptionStoreIds.ToArray();
                        }
                    }

                    //form fields
                    model.FirstName = customer.GetAttribute<string>(SystemCustomerAttributeNames.FirstName);
                    model.LastName = customer.GetAttribute<string>(SystemCustomerAttributeNames.LastName);
                    model.Gender = customer.GetAttribute<string>(SystemCustomerAttributeNames.Gender);
                    model.DateOfBirth = customer.GetAttribute<DateTime?>(SystemCustomerAttributeNames.DateOfBirth);
                    model.Company = customer.GetAttribute<string>(SystemCustomerAttributeNames.Company);
                    model.StreetAddress = customer.GetAttribute<string>(SystemCustomerAttributeNames.StreetAddress);
                    model.StreetAddress2 = customer.GetAttribute<string>(SystemCustomerAttributeNames.StreetAddress2);
                    model.ZipPostalCode = customer.GetAttribute<string>(SystemCustomerAttributeNames.ZipPostalCode);
                    model.City = customer.GetAttribute<string>(SystemCustomerAttributeNames.City);
                    model.CountryId = customer.GetAttribute<int>(SystemCustomerAttributeNames.CountryId);
                    model.StateProvinceId = customer.GetAttribute<int>(SystemCustomerAttributeNames.StateProvinceId);
                    model.Phone = customer.GetAttribute<string>(SystemCustomerAttributeNames.Phone);
                    model.Fax = customer.GetAttribute<string>(SystemCustomerAttributeNames.Fax);
                }
            }

            model.UsernamesEnabled = _customerSettings.UsernamesEnabled;
            model.AllowUsersToChangeUsernames = _customerSettings.AllowUsersToChangeUsernames;
            model.AllowCustomersToSetTimeZone = _dateTimeSettings.AllowCustomersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem { Text = tzi.DisplayName, Value = tzi.Id, Selected = (tzi.Id == model.TimeZoneId) });
            if (customer != null)
            {
                model.DisplayVatNumber = _taxSettings.EuVatEnabled;
            }
            else
            {
                model.DisplayVatNumber = false;
            }

            //vendors
            PrepareVendorsModel(model);
            //customer attributes
            PrepareCustomerAttributeModel(model, customer);

            model.GenderEnabled = _customerSettings.GenderEnabled;
            model.DateOfBirthEnabled = _customerSettings.DateOfBirthEnabled;
            model.CompanyEnabled = _customerSettings.CompanyEnabled;
            model.StreetAddressEnabled = _customerSettings.StreetAddressEnabled;
            model.StreetAddress2Enabled = _customerSettings.StreetAddress2Enabled;
            model.ZipPostalCodeEnabled = _customerSettings.ZipPostalCodeEnabled;
            model.CityEnabled = _customerSettings.CityEnabled;
            model.CountryEnabled = _customerSettings.CountryEnabled;
            model.StateProvinceEnabled = _customerSettings.StateProvinceEnabled;
            model.PhoneEnabled = _customerSettings.PhoneEnabled;
            model.FaxEnabled = _customerSettings.FaxEnabled;

            //countries and states
            if (_customerSettings.CountryEnabled)
            {
                model.AvailableCountries.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Address.SelectCountry"), Value = "0" });
                foreach (var c in _countryService.GetAllCountries(showHidden: true))
                {
                    model.AvailableCountries.Add(new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                        Selected = c.Id == model.CountryId
                    });
                }

                if (_customerSettings.StateProvinceEnabled)
                {
                    //states
                    var states = _stateProvinceService.GetStateProvincesByCountryId(model.CountryId).ToList();
                    if (states.Count > 0)
                    {
                        model.AvailableStates.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Address.SelectState"), Value = "0" });

                        foreach (var s in states)
                        {
                            model.AvailableStates.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString(), Selected = (s.Id == model.StateProvinceId) });
                        }
                    }
                    else
                    {
                        bool anyCountrySelected = model.AvailableCountries.Any(x => x.Selected);

                        model.AvailableStates.Add(new SelectListItem
                        {
                            Text = _localizationService.GetResource(anyCountrySelected ? "Admin.Address.OtherNonUS" : "Admin.Address.SelectState"),
                            Value = "0"
                        });
                    }
                }
            }

            //newsletter subscriptions
            model.AvailableNewsletterSubscriptionStores = allStores
                .Select(s => new CustomerModel.StoreModel() {Id = s.Id, Name = s.Name })
                .ToList();

            //customer roles
            model.AvailableCustomerRoles = _customerService
                .GetAllCustomerRoles(true)
                .Select(cr => cr.ToModel())
                .ToList();

            // Precheck Registered Role as a default role while creating a new customer through admin
            if(model.SelectedCustomerRoleIds==null && customer==null && model.AvailableCustomerRoles.Count>0)
            {
                model.SelectedCustomerRoleIds = new[] {model.AvailableCustomerRoles
                    .FirstOrDefault(c=>c.SystemName==SystemCustomerRoleNames.Registered).Id };
            }

            //reward points history
            if (customer != null)
            {
                model.DisplayRewardPointsHistory = _rewardPointsSettings.Enabled;
                model.AddRewardPointsValue = 0;
                model.AddRewardPointsMessage = "Some comment here...";

                //stores
                foreach (var store in allStores)
                {
                    model.RewardPointsAvailableStores.Add(new SelectListItem
                    {
                        Text = store.Name,
                        Value = store.Id.ToString(),
                        Selected = (store.Id == _storeContext.CurrentStore.Id)
                    });
                }
            }
            else
            {
                model.DisplayRewardPointsHistory = false;
            }
            //external authentication records
            if (customer != null)
            {
                model.AssociatedExternalAuthRecords = GetAssociatedExternalAuthRecords(customer);
            }
            //sending of the welcome message:
            //1. "admin approval" registration method
            //2. already created customer
            //3. registered
            model.AllowSendingOfWelcomeMessage = _customerSettings.UserRegistrationType == UserRegistrationType.AdminApproval &&
                customer != null &&
                customer.IsRegistered();
            //sending of the activation message
            //1. "email validation" registration method
            //2. already created customer
            //3. registered
            //4. not active
            model.AllowReSendingOfActivationMessage = _customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation &&
                customer != null &&
                customer.IsRegistered() &&
                !customer.Active;
        }

        [NonAction]
        protected virtual void PrepareAddressModel(CustomerAddressModel model, Address address, Customer customer, bool excludeProperties)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            model.CustomerId = customer.Id;
            if (address != null)
            {
                if (!excludeProperties)
                {
                    model.Address = address.ToModel();
                }
            }

            if (model.Address == null)
                model.Address = new AddressModel();

            model.Address.FirstNameEnabled = true;
            model.Address.FirstNameRequired = true;
            model.Address.LastNameEnabled = true;
            model.Address.LastNameRequired = true;
            model.Address.EmailEnabled = true;
            model.Address.EmailRequired = true;
            model.Address.CompanyEnabled = _addressSettings.CompanyEnabled;
            model.Address.CompanyRequired = _addressSettings.CompanyRequired;
            model.Address.CountryEnabled = _addressSettings.CountryEnabled;
            model.Address.StateProvinceEnabled = _addressSettings.StateProvinceEnabled;
            model.Address.CityEnabled = _addressSettings.CityEnabled;
            model.Address.CityRequired = _addressSettings.CityRequired;
            model.Address.StreetAddressEnabled = _addressSettings.StreetAddressEnabled;
            model.Address.StreetAddressRequired = _addressSettings.StreetAddressRequired;
            model.Address.StreetAddress2Enabled = _addressSettings.StreetAddress2Enabled;
            model.Address.StreetAddress2Required = _addressSettings.StreetAddress2Required;
            model.Address.ZipPostalCodeEnabled = _addressSettings.ZipPostalCodeEnabled;
            model.Address.ZipPostalCodeRequired = _addressSettings.ZipPostalCodeRequired;
            model.Address.PhoneEnabled = _addressSettings.PhoneEnabled;
            model.Address.PhoneRequired = _addressSettings.PhoneRequired;
            model.Address.FaxEnabled = _addressSettings.FaxEnabled;
            model.Address.FaxRequired = _addressSettings.FaxRequired;
            //countries
            model.Address.AvailableCountries.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Address.SelectCountry"), Value = "0" });
            foreach (var c in _countryService.GetAllCountries(showHidden: true))
                model.Address.AvailableCountries.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = (c.Id == model.Address.CountryId) });
            //states
            var states = model.Address.CountryId.HasValue ? _stateProvinceService.GetStateProvincesByCountryId(model.Address.CountryId.Value, showHidden: true).ToList() : new List<StateProvince>();
            if (states.Count > 0)
            {
                foreach (var s in states)
                    model.Address.AvailableStates.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString(), Selected = (s.Id == model.Address.StateProvinceId) });
            }
            else
                model.Address.AvailableStates.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Address.OtherNonUS"), Value = "0" });
            //customer attribute services
            model.Address.PrepareCustomAddressAttributes(address, _addressAttributeService, _addressAttributeParser);
        }

        #endregion

        #region Customers

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            //load registered customers by default
            var defaultRoleIds = new[] {_customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered).Id};
            var model = new CustomerListModel
            {
                UsernamesEnabled = _customerSettings.UsernamesEnabled,
                DateOfBirthEnabled = _customerSettings.DateOfBirthEnabled,
                CompanyEnabled = _customerSettings.CompanyEnabled,
                PhoneEnabled = _customerSettings.PhoneEnabled,
                ZipPostalCodeEnabled = _customerSettings.ZipPostalCodeEnabled,
                AvailableCustomerRoles = _customerService.GetAllCustomerRoles(true).Select(cr => cr.ToModel()).ToList(),
                SearchCustomerRoleIds = defaultRoleIds,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult CustomerList(DataSourceRequest command, CustomerListModel model,
            [ModelBinder(typeof(CommaSeparatedModelBinder))] int[] searchCustomerRoleIds)
        {
            //we use own own binder for searchCustomerRoleIds property 
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var searchDayOfBirth = 0;
            int searchMonthOfBirth = 0;
            if (!String.IsNullOrWhiteSpace(model.SearchDayOfBirth))
                searchDayOfBirth = Convert.ToInt32(model.SearchDayOfBirth);
            if (!String.IsNullOrWhiteSpace(model.SearchMonthOfBirth))
                searchMonthOfBirth = Convert.ToInt32(model.SearchMonthOfBirth);
            
            var customers = _customerService.GetAllCustomers(
                customerRoleIds: searchCustomerRoleIds,
                email: model.SearchEmail,
                username: model.SearchUsername,
                firstName: model.SearchFirstName,
                lastName: model.SearchLastName,
                dayOfBirth: searchDayOfBirth,
                monthOfBirth: searchMonthOfBirth,
                company: model.SearchCompany,
                phone: model.SearchPhone,
                zipPostalCode: model.SearchZipPostalCode,
                loadOnlyWithShoppingCart: false,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = customers.Select(PrepareCustomerModelForList),
                Total = customers.TotalCount
            };

            return Json(gridModel);
        }
        
        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var model = new CustomerModel();
            PrepareCustomerModel(model, null, false);
            //default value
            model.Active = true;
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateInput(false)]
        public ActionResult Create(CustomerModel model, bool continueEditing, FormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            if (!String.IsNullOrWhiteSpace(model.Email))
            {
                var cust2 = _customerService.GetCustomerByEmail(model.Email);
                if (cust2 != null)
                    ModelState.AddModelError("", "Email is already registered");
            }
            if (!String.IsNullOrWhiteSpace(model.Username) & _customerSettings.UsernamesEnabled)
            {
                var cust2 = _customerService.GetCustomerByUsername(model.Username);
                if (cust2 != null)
                    ModelState.AddModelError("", "Username is already registered");
            }

            //validate customer roles
            var allCustomerRoles = _customerService.GetAllCustomerRoles(true);
            var newCustomerRoles = new List<CustomerRole>();
            foreach (var customerRole in allCustomerRoles)
                if (model.SelectedCustomerRoleIds != null && model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                    newCustomerRoles.Add(customerRole);
            var customerRolesError = ValidateCustomerRoles(newCustomerRoles);
            if (!String.IsNullOrEmpty(customerRolesError))
            {
                ModelState.AddModelError("", customerRolesError);
                ErrorNotification(customerRolesError, false);
            }

            // Ensure that valid email address is entered if Registered role is checked to avoid registered customers with empty email address
            if (newCustomerRoles.Count > 0 && newCustomerRoles.FirstOrDefault(c => c.SystemName == SystemCustomerRoleNames.Registered) != null && !CommonHelper.IsValidEmail(model.Email))
            {
                ModelState.AddModelError("", "Valid Email is required for customer to be in 'Registered' role");
                ErrorNotification("Valid Email is required for customer to be in 'Registered' role", false);
            }

            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    CustomerGuid = Guid.NewGuid(),
                    Email = model.Email,
                    Username = model.Username,
                    VendorId = model.VendorId,
                    AdminComment = model.AdminComment,
                    IsTaxExempt = model.IsTaxExempt,
                    Active = model.Active,
                    CreatedOnUtc = DateTime.UtcNow,
                    LastActivityDateUtc = DateTime.UtcNow,
                };
                _customerService.InsertCustomer(customer);

                //form fields
                if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.TimeZoneId, model.TimeZoneId);
                if (_customerSettings.GenderEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Gender, model.Gender);
                _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.FirstName, model.FirstName);
                _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.LastName, model.LastName);
                if (_customerSettings.DateOfBirthEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.DateOfBirth, model.DateOfBirth);
                if (_customerSettings.CompanyEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Company, model.Company);
                if (_customerSettings.StreetAddressEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.StreetAddress, model.StreetAddress);
                if (_customerSettings.StreetAddress2Enabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.StreetAddress2, model.StreetAddress2);
                if (_customerSettings.ZipPostalCodeEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.ZipPostalCode, model.ZipPostalCode);
                if (_customerSettings.CityEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.City, model.City);
                if (_customerSettings.CountryEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.CountryId, model.CountryId);
                if (_customerSettings.CountryEnabled && _customerSettings.StateProvinceEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.StateProvinceId, model.StateProvinceId);
                if (_customerSettings.PhoneEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Phone, model.Phone);
                if (_customerSettings.FaxEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Fax, model.Fax);

                //custom customer attributes
                var customerAttributes = ParseCustomCustomerAttributes(customer, form);
                _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.CustomCustomerAttributes, customerAttributes);


                //newsletter subscriptions
                if (!String.IsNullOrEmpty(customer.Email))
                {
                    var allStores = _storeService.GetAllStores();
                    foreach (var store in allStores)
                    {
                        var newsletterSubscription = _newsLetterSubscriptionService
                            .GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, store.Id);
                        if (model.SelectedNewsletterSubscriptionStoreIds != null &&
                            model.SelectedNewsletterSubscriptionStoreIds.Contains(store.Id))
                        {
                            //subscribed
                            if (newsletterSubscription == null)
                            {
                                _newsLetterSubscriptionService.InsertNewsLetterSubscription(new NewsLetterSubscription
                                {
                                    NewsLetterSubscriptionGuid = Guid.NewGuid(),
                                    Email = customer.Email,
                                    Active = true,
                                    StoreId = store.Id,
                                    CreatedOnUtc = DateTime.UtcNow
                                });
                            }
                        }
                        else
                        {
                            //not subscribed
                            if (newsletterSubscription != null)
                            {
                                _newsLetterSubscriptionService.DeleteNewsLetterSubscription(newsletterSubscription);
                            }
                        }
                    }
                }

                //password
                if (!String.IsNullOrWhiteSpace(model.Password))
                {
                    var changePassRequest = new ChangePasswordRequest(model.Email, false, _customerSettings.DefaultPasswordFormat, model.Password);
                    var changePassResult = _customerRegistrationService.ChangePassword(changePassRequest);
                    if (!changePassResult.Success)
                    {
                        foreach (var changePassError in changePassResult.Errors)
                            ErrorNotification(changePassError);
                    }
                }

                //customer roles
                foreach (var customerRole in newCustomerRoles)
                {
                    //ensure that the current customer cannot add to "Administrators" system role if he's not an admin himself
                    if (customerRole.SystemName == SystemCustomerRoleNames.Administrators && 
                        !_workContext.CurrentCustomer.IsAdmin())
                        continue;

                    customer.CustomerRoles.Add(customerRole);
                }
                _customerService.UpdateCustomer(customer);
                

                //ensure that a customer with a vendor associated is not in "Administrators" role
                //otherwise, he won't be have access to the other functionality in admin area
                if (customer.IsAdmin() && customer.VendorId > 0)
                {
                    customer.VendorId = 0;
                    _customerService.UpdateCustomer(customer);
                    ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.AdminCouldNotbeVendor"));
                }

                //ensure that a customer in the Vendors role has a vendor account associated.
                //otherwise, he will have access to ALL products
                if (customer.IsVendor() && customer.VendorId == 0)
                {
                    var vendorRole = customer
                        .CustomerRoles
                        .FirstOrDefault(x => x.SystemName == SystemCustomerRoleNames.Vendors);
                    customer.CustomerRoles.Remove(vendorRole);
                    _customerService.UpdateCustomer(customer);
                    ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.CannotBeInVendoRoleWithoutVendorAssociated"));
                }

                //activity log
                _customerActivityService.InsertActivity("AddNewCustomer", _localizationService.GetResource("ActivityLog.AddNewCustomer"), customer.Id);

                SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = customer.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            PrepareCustomerModel(model, null, true);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(id);
            if (customer == null || customer.Deleted)
                //No customer found with the specified id
                return RedirectToAction("List");

            var model = new CustomerModel();
            PrepareCustomerModel(model, customer, false);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateInput(false)]
        public ActionResult Edit(CustomerModel model, bool continueEditing, FormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null || customer.Deleted)
                //No customer found with the specified id
                return RedirectToAction("List");

            //validate customer roles
            var allCustomerRoles = _customerService.GetAllCustomerRoles(true);
            var newCustomerRoles = new List<CustomerRole>();
            foreach (var customerRole in allCustomerRoles)
                if (model.SelectedCustomerRoleIds != null && model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                    newCustomerRoles.Add(customerRole);
            var customerRolesError = ValidateCustomerRoles(newCustomerRoles);
            if (!String.IsNullOrEmpty(customerRolesError))
            {
                ModelState.AddModelError("", customerRolesError);
                ErrorNotification(customerRolesError, false);
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    customer.AdminComment = model.AdminComment;
                    customer.IsTaxExempt = model.IsTaxExempt;
                    customer.Active = model.Active;
                    //email
                    if (!String.IsNullOrWhiteSpace(model.Email))
                    {
                        _customerRegistrationService.SetEmail(customer, model.Email);
                    }
                    else
                    {
                        customer.Email = model.Email;
                    }

                    //username
                    if (_customerSettings.UsernamesEnabled && _customerSettings.AllowUsersToChangeUsernames)
                    {
                        if (!String.IsNullOrWhiteSpace(model.Username))
                        {
                            _customerRegistrationService.SetUsername(customer, model.Username);
                        }
                        else
                        {
                            customer.Username = model.Username;
                        }
                    }

                    //VAT number
                    if (_taxSettings.EuVatEnabled)
                    {
                        var prevVatNumber = customer.GetAttribute<string>(SystemCustomerAttributeNames.VatNumber);

                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.VatNumber, model.VatNumber);
                        //set VAT number status
                        if (!String.IsNullOrEmpty(model.VatNumber))
                        {
                            if (!model.VatNumber.Equals(prevVatNumber, StringComparison.InvariantCultureIgnoreCase))
                            {
                                _genericAttributeService.SaveAttribute(customer, 
                                    SystemCustomerAttributeNames.VatNumberStatusId, 
                                    (int)_taxService.GetVatNumberStatus(model.VatNumber));
                            }
                        }
                        else
                        {
                            _genericAttributeService.SaveAttribute(customer,
                                SystemCustomerAttributeNames.VatNumberStatusId, 
                                (int)VatNumberStatus.Empty);
                        }
                    }

                    //vendor
                    customer.VendorId = model.VendorId;

                    //form fields
                    if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.TimeZoneId, model.TimeZoneId);
                    if (_customerSettings.GenderEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Gender, model.Gender);
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.FirstName, model.FirstName);
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.LastName, model.LastName);
                    if (_customerSettings.DateOfBirthEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.DateOfBirth, model.DateOfBirth);
                    if (_customerSettings.CompanyEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Company, model.Company);
                    if (_customerSettings.StreetAddressEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.StreetAddress, model.StreetAddress);
                    if (_customerSettings.StreetAddress2Enabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.StreetAddress2, model.StreetAddress2);
                    if (_customerSettings.ZipPostalCodeEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.ZipPostalCode, model.ZipPostalCode);
                    if (_customerSettings.CityEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.City, model.City);
                    if (_customerSettings.CountryEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.CountryId, model.CountryId);
                    if (_customerSettings.CountryEnabled && _customerSettings.StateProvinceEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.StateProvinceId, model.StateProvinceId);
                    if (_customerSettings.PhoneEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Phone, model.Phone);
                    if (_customerSettings.FaxEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Fax, model.Fax);

                    //custom customer attributes
                    var customerAttributes = ParseCustomCustomerAttributes(customer, form);
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.CustomCustomerAttributes, customerAttributes);

                    //newsletter subscriptions
                    if (!String.IsNullOrEmpty(customer.Email))
                    {
                        var allStores = _storeService.GetAllStores();
                        foreach (var store in allStores)
                        {
                            var newsletterSubscription = _newsLetterSubscriptionService
                                .GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, store.Id);
                            if (model.SelectedNewsletterSubscriptionStoreIds != null &&
                                model.SelectedNewsletterSubscriptionStoreIds.Contains(store.Id))
                            {
                                //subscribed
                                if (newsletterSubscription == null)
                                {
                                    _newsLetterSubscriptionService.InsertNewsLetterSubscription(new NewsLetterSubscription
                                    {
                                        NewsLetterSubscriptionGuid = Guid.NewGuid(),
                                        Email = customer.Email,
                                        Active = true,
                                        StoreId = store.Id,
                                        CreatedOnUtc = DateTime.UtcNow
                                    });
                                }
                            }
                            else
                            {
                                //not subscribed
                                if (newsletterSubscription != null)
                                {
                                    _newsLetterSubscriptionService.DeleteNewsLetterSubscription(newsletterSubscription);
                                }
                            }
                        }
                    }


                    //customer roles
                    foreach (var customerRole in allCustomerRoles)
                    {
                        //ensure that the current customer cannot add/remove to/from "Administrators" system role
                        //if he's not an admin himself
                        if (customerRole.SystemName == SystemCustomerRoleNames.Administrators &&
                            !_workContext.CurrentCustomer.IsAdmin())
                            continue;

                        if (model.SelectedCustomerRoleIds != null &&
                            model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                        {
                            //new role
                            if (customer.CustomerRoles.Count(cr => cr.Id == customerRole.Id) == 0)
                                customer.CustomerRoles.Add(customerRole);
                        }
                        else
                        {
                            //remove role
                            if (customer.CustomerRoles.Count(cr => cr.Id == customerRole.Id) > 0)
                                customer.CustomerRoles.Remove(customerRole);
                        }
                    }
                    _customerService.UpdateCustomer(customer);
                    

                    //ensure that a customer with a vendor associated is not in "Administrators" role
                    //otherwise, he won't have access to the other functionality in admin area
                    if (customer.IsAdmin() && customer.VendorId > 0)
                    {
                        customer.VendorId = 0;
                        _customerService.UpdateCustomer(customer);
                        ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.AdminCouldNotbeVendor"));
                    }

                    //ensure that a customer in the Vendors role has a vendor account associated.
                    //otherwise, he will have access to ALL products
                    if (customer.IsVendor() && customer.VendorId == 0)
                    {
                        var vendorRole = customer
                            .CustomerRoles
                            .FirstOrDefault(x => x.SystemName == SystemCustomerRoleNames.Vendors);
                        customer.CustomerRoles.Remove(vendorRole);
                        _customerService.UpdateCustomer(customer);
                        ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.CannotBeInVendoRoleWithoutVendorAssociated"));
                    }


                    //activity log
                    _customerActivityService.InsertActivity("EditCustomer", _localizationService.GetResource("ActivityLog.EditCustomer"), customer.Id);

                    SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.Updated"));
                    if (continueEditing)
                    {
                        //selected tab
                        SaveSelectedTabIndex();

                        return RedirectToAction("Edit",  new {id = customer.Id});
                    }
                    return RedirectToAction("List");
                }
                catch (Exception exc)
                {
                    ErrorNotification(exc.Message, false);
                }
            }


            //If we got this far, something failed, redisplay form
            PrepareCustomerModel(model, customer, true);
            return View(model);
        }
        
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("changepassword")]
        public ActionResult ChangePassword(CustomerModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var changePassRequest = new ChangePasswordRequest(model.Email,
                    false, _customerSettings.DefaultPasswordFormat, model.Password);
                var changePassResult = _customerRegistrationService.ChangePassword(changePassRequest);
                if (changePassResult.Success)
                    SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.PasswordChanged"));
                else
                    foreach (var error in changePassResult.Errors)
                        ErrorNotification(error);
            }

            return RedirectToAction("Edit",  new {id = customer.Id});
        }
        
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("markVatNumberAsValid")]
        public ActionResult MarkVatNumberAsValid(CustomerModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            _genericAttributeService.SaveAttribute(customer, 
                SystemCustomerAttributeNames.VatNumberStatusId,
                (int)VatNumberStatus.Valid);

            return RedirectToAction("Edit",  new {id = customer.Id});
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("markVatNumberAsInvalid")]
        public ActionResult MarkVatNumberAsInvalid(CustomerModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            _genericAttributeService.SaveAttribute(customer,
                SystemCustomerAttributeNames.VatNumberStatusId,
                (int)VatNumberStatus.Invalid);
            
            return RedirectToAction("Edit",  new {id = customer.Id});
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("remove-affiliate")]
        public ActionResult RemoveAffiliate(CustomerModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");
            
            customer.AffiliateId = 0;
            _customerService.UpdateCustomer(customer);

            return RedirectToAction("Edit", new { id = customer.Id });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            try
            {
                _customerService.DeleteCustomer(customer);

                //remove newsletter subscription (if exists)
                foreach (var store in _storeService.GetAllStores())
                {
                    var subscription = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, store.Id);
                    if (subscription != null)
                        _newsLetterSubscriptionService.DeleteNewsLetterSubscription(subscription);
                }

                //activity log
                _customerActivityService.InsertActivity("DeleteCustomer", _localizationService.GetResource("ActivityLog.DeleteCustomer"), customer.Id);

                SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.Deleted"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = customer.Id });
            }
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("impersonate")]
        public ActionResult Impersonate(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.AllowCustomerImpersonation))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            //ensure that a non-admin user cannot impersonate as an administrator
            //otherwise, that user can simply impersonate as an administrator and gain additional administrative privileges
            if (!_workContext.CurrentCustomer.IsAdmin() && customer.IsAdmin())
            {
                ErrorNotification("A non-admin user cannot impersonate as an administrator");
                return RedirectToAction("Edit", customer.Id);
            }


            _genericAttributeService.SaveAttribute<int?>(_workContext.CurrentCustomer,
                SystemCustomerAttributeNames.ImpersonatedCustomerId, customer.Id);

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("send-welcome-message")]
        public ActionResult SendWelcomeMessage(CustomerModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            _workflowMessageService.SendCustomerWelcomeMessage(customer, _workContext.WorkingLanguage.Id);

            SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.SendWelcomeMessage.Success"));

            return RedirectToAction("Edit", new { id = customer.Id });
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("resend-activation-message")]
        public ActionResult ReSendActivationMessage(CustomerModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            //email validation message
            _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.AccountActivationToken, Guid.NewGuid().ToString());
            _workflowMessageService.SendCustomerEmailValidationMessage(customer, _workContext.WorkingLanguage.Id);

            SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.ReSendActivationMessage.Success"));

            return RedirectToAction("Edit", new { id = customer.Id });
        }

        public ActionResult SendEmail(CustomerModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            try
            {
                if (String.IsNullOrWhiteSpace(customer.Email))
                    throw new NopException("Customer email is empty");
                if (!CommonHelper.IsValidEmail(customer.Email))
                    throw new NopException("Customer email is not valid");
                if (String.IsNullOrWhiteSpace(model.SendEmail.Subject))
                    throw new NopException("Email subject is empty");
                if (String.IsNullOrWhiteSpace(model.SendEmail.Body))
                    throw new NopException("Email body is empty");

                var emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
                if (emailAccount == null)
                    emailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
                if (emailAccount == null)
                    throw new NopException("Email account can't be loaded");
                var email = new QueuedEmail
                {
                    Priority = QueuedEmailPriority.High,
                    EmailAccountId = emailAccount.Id,
                    FromName = emailAccount.DisplayName,
                    From = emailAccount.Email,
                    ToName = customer.GetFullName(),
                    To = customer.Email,
                    Subject = model.SendEmail.Subject,
                    Body = model.SendEmail.Body,
                    CreatedOnUtc = DateTime.UtcNow,
                    DontSendBeforeDateUtc = (model.SendEmail.SendImmediately || !model.SendEmail.DontSendBeforeDate.HasValue) ? 
                        null : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.SendEmail.DontSendBeforeDate.Value)
                };
                _queuedEmailService.InsertQueuedEmail(email);
                SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.SendEmail.Queued"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
            }

            return RedirectToAction("Edit", new { id = customer.Id });
        }

        public ActionResult SendPm(CustomerModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            try
            {
                if (!_forumSettings.AllowPrivateMessages)
                    throw new NopException("Private messages are disabled");
                if (customer.IsGuest())
                    throw new NopException("Customer should be registered");
                if (String.IsNullOrWhiteSpace(model.SendPm.Subject))
                    throw new NopException("PM subject is empty");
                if (String.IsNullOrWhiteSpace(model.SendPm.Message))
                    throw new NopException("PM message is empty");


                var privateMessage = new PrivateMessage
                {
                    StoreId = _storeContext.CurrentStore.Id,
                    ToCustomerId = customer.Id,
                    FromCustomerId = _workContext.CurrentCustomer.Id,
                    Subject = model.SendPm.Subject,
                    Text = model.SendPm.Message,
                    IsDeletedByAuthor = false,
                    IsDeletedByRecipient = false,
                    IsRead = false,
                    CreatedOnUtc = DateTime.UtcNow
                };

                _forumService.InsertPrivateMessage(privateMessage);
                SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.SendPM.Sent"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
            }

            return RedirectToAction("Edit", new { id = customer.Id });
        }
        
        #endregion
        
        #region Reward points history

        [HttpPost]
        public ActionResult RewardPointsHistorySelect(DataSourceRequest command, int customerId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null)
                throw new ArgumentException("No customer found with the specified id");

            var rewardPoints = _rewardPointService.GetRewardPointsHistory(customer.Id, true, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = rewardPoints.Select(rph =>
                {
                    var store = _storeService.GetStoreById(rph.StoreId);
                    return new CustomerModel.RewardPointsHistoryModel
                    {
                        StoreName = store != null ? store.Name : "Unknown",
                        Points = rph.Points,
                        PointsBalance = rph.PointsBalance,
                        Message = rph.Message,
                        CreatedOn = _dateTimeHelper.ConvertToUserTime(rph.CreatedOnUtc, DateTimeKind.Utc)
                    };
                }),
                Total = rewardPoints.TotalCount
            };

            return Json(gridModel);
        }

        [ValidateInput(false)]
        public ActionResult RewardPointsHistoryAdd(int customerId, int storeId, int addRewardPointsValue, string addRewardPointsMessage)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null)
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);

            _rewardPointService.AddRewardPointsHistoryEntry(customer,
                addRewardPointsValue, storeId, addRewardPointsMessage);

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }
        
        #endregion
        
        #region Addresses

        [HttpPost]
        public ActionResult AddressesSelect(int customerId, DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null)
                throw new ArgumentException("No customer found with the specified id", "customerId");

            var addresses = customer.Addresses.OrderByDescending(a => a.CreatedOnUtc).ThenByDescending(a => a.Id).ToList();
            var gridModel = new DataSourceResult
            {
                Data = addresses.Select(x =>
                {
                    var model = x.ToModel();
                    var addressHtmlSb = new StringBuilder("<div>");
                    if (_addressSettings.CompanyEnabled && !String.IsNullOrEmpty(model.Company))
                        addressHtmlSb.AppendFormat("{0}<br />", Server.HtmlEncode(model.Company));
                    if (_addressSettings.StreetAddressEnabled && !String.IsNullOrEmpty(model.Address1))
                        addressHtmlSb.AppendFormat("{0}<br />", Server.HtmlEncode(model.Address1));
                    if (_addressSettings.StreetAddress2Enabled && !String.IsNullOrEmpty(model.Address2))
                        addressHtmlSb.AppendFormat("{0}<br />", Server.HtmlEncode(model.Address2));
                    if (_addressSettings.CityEnabled && !String.IsNullOrEmpty(model.City))
                        addressHtmlSb.AppendFormat("{0},", Server.HtmlEncode(model.City));
                    if (_addressSettings.StateProvinceEnabled && !String.IsNullOrEmpty(model.StateProvinceName))
                        addressHtmlSb.AppendFormat("{0},", Server.HtmlEncode(model.StateProvinceName));
                    if (_addressSettings.ZipPostalCodeEnabled && !String.IsNullOrEmpty(model.ZipPostalCode))
                        addressHtmlSb.AppendFormat("{0}<br />", Server.HtmlEncode(model.ZipPostalCode));
                    if (_addressSettings.CountryEnabled && !String.IsNullOrEmpty(model.CountryName))
                        addressHtmlSb.AppendFormat("{0}", Server.HtmlEncode(model.CountryName));
                    var customAttributesFormatted = _addressAttributeFormatter.FormatAttributes(x.CustomAttributes);
                    if (!String.IsNullOrEmpty(customAttributesFormatted))
                    {
                        //already encoded
                        addressHtmlSb.AppendFormat("<br />{0}", customAttributesFormatted);
                    }
                    addressHtmlSb.Append("</div>");
                    model.AddressHtml = addressHtmlSb.ToString();
                    return model;
                }),
                Total = addresses.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult AddressDelete(int id, int customerId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null)
                throw new ArgumentException("No customer found with the specified id", "customerId");

            var address = customer.Addresses.FirstOrDefault(a => a.Id == id);
            if (address == null)
                //No customer found with the specified id
                return Content("No customer found with the specified id");
            customer.RemoveAddress(address);
            _customerService.UpdateCustomer(customer);
            //now delete the address record
            _addressService.DeleteAddress(address);

            return new NullJsonResult();
        }
        
        public ActionResult AddressCreate(int customerId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            var model = new CustomerAddressModel();
            PrepareAddressModel(model, null, customer, false);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddressCreate(CustomerAddressModel model, FormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(model.CustomerId);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            //custom address attributes
            var customAttributes = form.ParseCustomAddressAttributes(_addressAttributeParser, _addressAttributeService);
            var customAttributeWarnings = _addressAttributeParser.GetAttributeWarnings(customAttributes);
            foreach (var error in customAttributeWarnings)
            {
                ModelState.AddModelError("", error);
            }

            if (ModelState.IsValid)
            {
                var address = model.Address.ToEntity();
                address.CustomAttributes = customAttributes;
                address.CreatedOnUtc = DateTime.UtcNow;
                //some validation
                if (address.CountryId == 0)
                    address.CountryId = null;
                if (address.StateProvinceId == 0)
                    address.StateProvinceId = null;
                customer.Addresses.Add(address);
                _customerService.UpdateCustomer(customer);

                SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.Addresses.Added"));
                return RedirectToAction("AddressEdit", new { addressId = address.Id, customerId = model.CustomerId });
            }

            //If we got this far, something failed, redisplay form
            PrepareAddressModel(model, null, customer, true);
            return View(model);
        }

        public ActionResult AddressEdit(int addressId, int customerId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            var address = _addressService.GetAddressById(addressId);
            if (address == null)
                //No address found with the specified id
                return RedirectToAction("Edit", new { id = customer.Id });

            var model = new CustomerAddressModel();
            PrepareAddressModel(model, address, customer, false);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddressEdit(CustomerAddressModel model, FormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customer = _customerService.GetCustomerById(model.CustomerId);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            var address = _addressService.GetAddressById(model.Address.Id);
            if (address == null)
                //No address found with the specified id
                return RedirectToAction("Edit", new { id = customer.Id });

            //custom address attributes
            var customAttributes = form.ParseCustomAddressAttributes(_addressAttributeParser, _addressAttributeService);
            var customAttributeWarnings = _addressAttributeParser.GetAttributeWarnings(customAttributes);
            foreach (var error in customAttributeWarnings)
            {
                ModelState.AddModelError("", error);
            }

            if (ModelState.IsValid)
            {
                address = model.Address.ToEntity(address);
                address.CustomAttributes = customAttributes;
                _addressService.UpdateAddress(address);

                SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.Addresses.Updated"));
                return RedirectToAction("AddressEdit", new { addressId = model.Address.Id, customerId = model.CustomerId });
            }

            //If we got this far, something failed, redisplay form
            PrepareAddressModel(model, address, customer, true);

            return View(model);
        }

        #endregion

        #region Orders
        
        [HttpPost]
        public ActionResult OrderList(int customerId, DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var orders = _orderService.SearchOrders(customerId: customerId);

            var gridModel = new DataSourceResult
            {
                Data = orders.PagedForCommand(command)
                    .Select(order =>
                    {
                        var store = _storeService.GetStoreById(order.StoreId);
                        var orderModel = new CustomerModel.OrderModel
                        {
                            Id = order.Id, 
                            OrderStatus = order.OrderStatus.GetLocalizedEnum(_localizationService, _workContext),
                            PaymentStatus = order.PaymentStatus.GetLocalizedEnum(_localizationService, _workContext),
                            ShippingStatus = order.ShippingStatus.GetLocalizedEnum(_localizationService, _workContext),
                            OrderTotal = _priceFormatter.FormatPrice(order.OrderTotal, true, false),
                            StoreName = store != null ? store.Name : "Unknown",
                            CreatedOn = _dateTimeHelper.ConvertToUserTime(order.CreatedOnUtc, DateTimeKind.Utc),
                        };
                        return orderModel;
                    }),
                Total = orders.Count
            };


            return Json(gridModel);
        }
        
        #endregion

        #region Reports

        public ActionResult Reports()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var model = new CustomerReportsModel();
            //customers by number of orders
            model.BestCustomersByNumberOfOrders = new BestCustomersReportModel();
            model.BestCustomersByNumberOfOrders.AvailableOrderStatuses = OrderStatus.Pending.ToSelectList(false).ToList();
            model.BestCustomersByNumberOfOrders.AvailableOrderStatuses.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            model.BestCustomersByNumberOfOrders.AvailablePaymentStatuses = PaymentStatus.Pending.ToSelectList(false).ToList();
            model.BestCustomersByNumberOfOrders.AvailablePaymentStatuses.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            model.BestCustomersByNumberOfOrders.AvailableShippingStatuses = ShippingStatus.NotYetShipped.ToSelectList(false).ToList();
            model.BestCustomersByNumberOfOrders.AvailableShippingStatuses.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

            //customers by order total
            model.BestCustomersByOrderTotal = new BestCustomersReportModel();
            model.BestCustomersByOrderTotal.AvailableOrderStatuses = OrderStatus.Pending.ToSelectList(false).ToList();
            model.BestCustomersByOrderTotal.AvailableOrderStatuses.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            model.BestCustomersByOrderTotal.AvailablePaymentStatuses = PaymentStatus.Pending.ToSelectList(false).ToList();
            model.BestCustomersByOrderTotal.AvailablePaymentStatuses.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            model.BestCustomersByOrderTotal.AvailableShippingStatuses = ShippingStatus.NotYetShipped.ToSelectList(false).ToList();
            model.BestCustomersByOrderTotal.AvailableShippingStatuses.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            
            return View(model);
        }

        [HttpPost]
        public ActionResult ReportBestCustomersByOrderTotalList(DataSourceRequest command, BestCustomersReportModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return Content("");

            DateTime? startDateValue = (model.StartDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.EndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            OrderStatus? orderStatus = model.OrderStatusId > 0 ? (OrderStatus?)(model.OrderStatusId) : null;
            PaymentStatus? paymentStatus = model.PaymentStatusId > 0 ? (PaymentStatus?)(model.PaymentStatusId) : null;
            ShippingStatus? shippingStatus = model.ShippingStatusId > 0 ? (ShippingStatus?)(model.ShippingStatusId) : null;


            var items = _customerReportService.GetBestCustomersReport(startDateValue, endDateValue,
                orderStatus, paymentStatus, shippingStatus, 1, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = items.Select(x =>
                {
                    var m = new BestCustomerReportLineModel
                    {
                        CustomerId = x.CustomerId,
                        OrderTotal = _priceFormatter.FormatPrice(x.OrderTotal, true, false),
                        OrderCount = x.OrderCount,
                    };
                    var customer = _customerService.GetCustomerById(x.CustomerId);
                    if (customer != null)
                    {
                        m.CustomerName = customer.IsRegistered() ? customer.Email : _localizationService.GetResource("Admin.Customers.Guest");
                    }
                    return m;
                }),
                Total = items.TotalCount
            };

            return Json(gridModel);
        }
        [HttpPost]
        public ActionResult ReportBestCustomersByNumberOfOrdersList(DataSourceRequest command, BestCustomersReportModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return Content("");

            DateTime? startDateValue = (model.StartDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.EndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            OrderStatus? orderStatus = model.OrderStatusId > 0 ? (OrderStatus?)(model.OrderStatusId) : null;
            PaymentStatus? paymentStatus = model.PaymentStatusId > 0 ? (PaymentStatus?)(model.PaymentStatusId) : null;
            ShippingStatus? shippingStatus = model.ShippingStatusId > 0 ? (ShippingStatus?)(model.ShippingStatusId) : null;


            var items = _customerReportService.GetBestCustomersReport(startDateValue, endDateValue,
                orderStatus, paymentStatus, shippingStatus, 2, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = items.Select(x =>
                {
                    var m = new BestCustomerReportLineModel
                    {
                        CustomerId = x.CustomerId,
                        OrderTotal = _priceFormatter.FormatPrice(x.OrderTotal, true, false),
                        OrderCount = x.OrderCount,
                    };
                    var customer = _customerService.GetCustomerById(x.CustomerId);
                    if (customer != null)
                    {
                        m.CustomerName = customer.IsRegistered() ? customer.Email : _localizationService.GetResource("Admin.Customers.Guest");
                    }
                    return m;
                }),
                Total = items.TotalCount
            };

            return Json(gridModel);
        }

        [ChildActionOnly]
        public ActionResult ReportRegisteredCustomers()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return Content("");

            return PartialView();
        }
        [HttpPost]
        public ActionResult ReportRegisteredCustomersList(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return Content("");

            var model = GetReportRegisteredCustomersModel();
            var gridModel = new DataSourceResult
            {
                Data = model,
                Total = model.Count
            };

            return Json(gridModel);
        }
        
        #endregion

        #region Current shopping cart/ wishlist

        [HttpPost]
        public ActionResult GetCartList(int customerId, int cartTypeId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return Content("");

            var customer = _customerService.GetCustomerById(customerId);
            var cart = customer.ShoppingCartItems.Where(x => x.ShoppingCartTypeId == cartTypeId).ToList();

            var gridModel = new DataSourceResult
            {
                Data = cart.Select(sci =>
                {
                    decimal taxRate;
                    var store = _storeService.GetStoreById(sci.StoreId); 
                    var sciModel = new ShoppingCartItemModel
                    {
                        Id = sci.Id,
                        Store = store != null ? store.Name : "Unknown",
                        ProductId = sci.ProductId,
                        Quantity = sci.Quantity,
                        ProductName = sci.Product.Name,
                        AttributeInfo = _productAttributeFormatter.FormatAttributes(sci.Product, sci.AttributesXml),
                        UnitPrice = _priceFormatter.FormatPrice(_taxService.GetProductPrice(sci.Product, _priceCalculationService.GetUnitPrice(sci), out taxRate)),
                        Total = _priceFormatter.FormatPrice(_taxService.GetProductPrice(sci.Product, _priceCalculationService.GetSubTotal(sci), out taxRate)),
                        UpdatedOn = _dateTimeHelper.ConvertToUserTime(sci.UpdatedOnUtc, DateTimeKind.Utc)
                    };
                    return sciModel;
                }),
                Total = cart.Count
            };

            return Json(gridModel);
        }

        #endregion

        #region Activity log

        [HttpPost]
        public ActionResult ListActivityLog(DataSourceRequest command, int customerId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return Content("");

            var activityLog = _customerActivityService.GetAllActivities(null, null, customerId, 0, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = activityLog.Select(x =>
                {
                    var m = new CustomerModel.ActivityLogModel
                    {
                        Id = x.Id,
                        ActivityLogTypeName = x.ActivityLogType.Name,
                        Comment = x.Comment,
                        CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc)
                    };
                    return m;

                }),
                Total = activityLog.TotalCount
            };

            return Json(gridModel);
        }

        #endregion

        #region Back in stock subscriptions

        [HttpPost]
        public ActionResult BackInStockSubscriptionList(DataSourceRequest command, int customerId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return Content("");

            var subscriptions = _backInStockSubscriptionService.GetAllSubscriptionsByCustomerId(customerId,
                0, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = subscriptions.Select(x =>
                {
                    var store = _storeService.GetStoreById(x.StoreId);
                    var product = x.Product;
                    var m = new CustomerModel.BackInStockSubscriptionModel
                    {
                        Id = x.Id,
                        StoreName = store != null ? store.Name : "Unknown",
                        ProductId = x.ProductId,
                        ProductName = product != null ? product.Name : "Unknown",
                        CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc)
                    };
                    return m;

                }),
                Total = subscriptions.TotalCount
            };

            return Json(gridModel);
        }

        #endregion

        #region Export / Import

        [HttpPost, ActionName("List")]
        [FormValueRequired("exportexcel-all")]
        public ActionResult ExportExcelAll(CustomerListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var searchDayOfBirth = 0;
            int searchMonthOfBirth = 0;
            if (!String.IsNullOrWhiteSpace(model.SearchDayOfBirth))
                searchDayOfBirth = Convert.ToInt32(model.SearchDayOfBirth);
            if (!String.IsNullOrWhiteSpace(model.SearchMonthOfBirth))
                searchMonthOfBirth = Convert.ToInt32(model.SearchMonthOfBirth);

            var customers = _customerService.GetAllCustomers(
                customerRoleIds: model.SearchCustomerRoleIds,
                email: model.SearchEmail,
                username: model.SearchUsername,
                firstName: model.SearchFirstName,
                lastName: model.SearchLastName,
                dayOfBirth: searchDayOfBirth,
                monthOfBirth: searchMonthOfBirth,
                company: model.SearchCompany,
                phone: model.SearchPhone,
                zipPostalCode: model.SearchZipPostalCode,
                loadOnlyWithShoppingCart: false);

            try
            {
                byte[] bytes = _exportManager.ExportCustomersToXlsx(customers);
                return File(bytes, "text/xls", "customers.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public ActionResult ExportExcelSelected(string selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customers = new List<Customer>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();
                customers.AddRange(_customerService.GetCustomersByIds(ids));
            }

            try
            {
                byte[] bytes = _exportManager.ExportCustomersToXlsx(customers);
                return File(bytes, "text/xls", "customers.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost, ActionName("List")]
        [FormValueRequired("exportxml-all")]
        public ActionResult ExportXmlAll(CustomerListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var searchDayOfBirth = 0;
            int searchMonthOfBirth = 0;
            if (!String.IsNullOrWhiteSpace(model.SearchDayOfBirth))
                searchDayOfBirth = Convert.ToInt32(model.SearchDayOfBirth);
            if (!String.IsNullOrWhiteSpace(model.SearchMonthOfBirth))
                searchMonthOfBirth = Convert.ToInt32(model.SearchMonthOfBirth);

            var customers = _customerService.GetAllCustomers(
                customerRoleIds: model.SearchCustomerRoleIds,
                email: model.SearchEmail,
                username: model.SearchUsername,
                firstName: model.SearchFirstName,
                lastName: model.SearchLastName,
                dayOfBirth: searchDayOfBirth,
                monthOfBirth: searchMonthOfBirth,
                company: model.SearchCompany,
                phone: model.SearchPhone,
                zipPostalCode: model.SearchZipPostalCode,
                loadOnlyWithShoppingCart: false);

            try
            {
                var xml = _exportManager.ExportCustomersToXml(customers);
                return new XmlDownloadResult(xml, "customers.xml");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public ActionResult ExportXmlSelected(string selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var customers = new List<Customer>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();
                customers.AddRange(_customerService.GetCustomersByIds(ids));
            }

            var xml = _exportManager.ExportCustomersToXml(customers);
            return new XmlDownloadResult(xml, "customers.xml");
        }

        #endregion
    }
}

var tablesWithTitle = [
'Catalog'
,'CatalogProductPrice'
,'CatalogProductPriceHistory'
,'CompanyConfiguration'
,'CompanyCreditTier'
,'CompanyLocation'
,'CompanyUser'
,'CompanyUserCredit'
,'CompanyUserCreditHistory'
,'Invoice'
,'InvoiceOrder'
,'Order'
,'OrderDeliveryPartner'
,'OrderPayment'
,'OrderProduct'
,'OtpServer'
,'Product'
,'ProductBrand'
,'ProductCategory'
,'ProductInventoryHistory'
,'UserAddress'
,'UserGroup'
,'UserGroupUser'
,'UserLoyaltyPointHistory'
];


for (let index = 0; index < tablesWithTitle.length; index++) {
    const element = tablesWithTitle[index];

    console.log(`${element}ListScreenComponent,`);
    console.log(`${element}CrudModalComponent,`);
    
}
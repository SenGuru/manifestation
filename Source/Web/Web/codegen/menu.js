//  host
// var tablesWithTitle = [
//     { title: 'Clients', table: 'Company' },
//     { title: 'Orders', table: 'Order' },
//     { title: 'Invoices', table: 'Invoice' },
//     { title: 'Employees', table: 'User' },
//     { title: 'Products', table: 'Product' },
//     { title: 'Reports', table: 'Report' },
//     { title: 'Audit', table: 'AuditOperation' },
//     { title: 'Loyalty Point Transactions', table: 'UserLoyaltyPointHistory' },
//     { title: 'Delivery Status', table: 'Order' },
//     { title: 'Jobs', table: 'Job' },
// ];

// client
// var tablesWithTitle = [
//     { title: 'User', table: 'User' },
//     { title: 'Orders', table: 'Order' },
//     { title: 'Approvals', table: 'OrderDeliveryPartner' },
//     { title: 'Invoices', table: 'Invoice' },
//     { title: 'Product Pricing', table: 'pricing' },
//     { title: 'Reports', table: 'Report' },
//     { title: 'Audit', table: 'AuditOperation' },
// ];

// third party
// var tablesWithTitle = [
//     { title: 'Clients', table: 'Company' },
//     { title: 'Orders', table: 'Order' },
//     { title: 'Invoices', table: 'Invoice' },
//     { title: 'User', table: 'User' },
//     { title: 'Products Or Catalogs', table: 'Product' },
//     { title: 'Reports', table: 'Report' },
//     { title: 'Audit', table: 'AuditOperation' },
//     { title: 'Delivery Status', table: 'Order' },
//     { title: 'Jobs', table: 'Job' },
// ];

// Employee
// var tablesWithTitle = [
//     { title: 'Host Catalog View', table: 'ProductCategory' },
//     { title: 'Host Place Order', table: 'Order' },
//     { title: 'Third Party Catalog View', table: 'InvoiceOrder' },
//     { title: 'Third Party Place Order', table: 'OrderPayment' },
//     { title: 'Loyalty Point Transactions', table: 'UserLoyaltyPointHistory' },
//     { title: 'Reports', table: 'Report' },
//     { title: 'Profile', table: 'Profile' },
// ];

var tables = [
    'DailyAffirmationText',
'DefaultCategory',
'DefaultDailyAffirmationText',
'DefaultGoalTrackerCategory',
'DefaultGoalTracker',
'DefaultVisionBoardPicture',
];


// for (let index = 0; index < tablesWithTitle.length; index++) {
//     const element = tablesWithTitle[index];

//     console.log(`${element}ListScreenComponent,`);
//     console.log(`${element}CrudModalComponent,`);

// }

console.log(`export const SUPER_ADMIN_MENU: MenuItem[] = [`)
for (let index = 0; index < tablesWithTitle.length; index++) {
    const element = tablesWithTitle[index];
    const tableWithUpperCase = element.table.toUpperCase();
    const titleWithLowerCase = String(element.title.toLowerCase());
    let routingPath = titleWithLowerCase.replace(" ", "-");
    routingPath = routingPath.replace(" ", "-");
    routingPath = routingPath.replace(" ", "-");
    console.log(`                 {
                 id: ${index+1},
                 label: 'MENUITEMS.${tableWithUpperCase}.TEXT',
                 icon: 'ri-building-line',
                 link: '/sa/${routingPath}',
                 },`)
}
console.log(' ];');

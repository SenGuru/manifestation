var screenType = "Catalog";
var tablesWithTitle = [
    { title: 'Catalog Details', table: 'Catalog' },
    { title: 'Products', table: 'Product' },
    { title: 'Customers', table: 'Company' },
];


function splitCamelCaseToString(s) {
    return s.split(/(?=[A-Z])/).join('-').toLowerCase();
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}


console.log("//////////////////////////////////////////////////////////////////////////////////////////////////////////");
console.log("//////////////////////////////////////////////////////////////////////////////////////////////////////////");
console.log("/// In header details screen");

console.log(`<div class="col-xxl-12">
<div class="card mb-0 shadow-none bg-transparent">
    <div class="card-header align-items-center d-flex bg-transparent">
        <div class="flex-shrink-0 ms-2">
            <ul ngbNav
                class="nav justify-content-end nav-tabs-custom rounded card-header-tabs border-bottom-0">`)
for (const element of tablesWithTitle) {
    const tableWithUpperCase = element.table.toUpperCase();
    const titleWithLowerCase = String(element.title.toLowerCase());
    let routingPath = titleWithLowerCase.replace(" ", "-");
    routingPath = routingPath.replace(" ", "-");
    routingPath = routingPath.replace(" ", "-");
    console.log(`
                    <li class="nav-item cursor-pointer" (click)="changeTab('${routingPath}')">
                        <a class="nav-link bg-transparent pt-0 pb-2"
                            [ngClass]="selected${screenType}ConfigScreenType == '${routingPath}' ? 'active' : ''">
                            ${element.title}
                        </a>
                    </li> 
                     `)
}
console.log(`
                        </ul>
                    </div>
                </div>
                </div>
            </div>
            `);


console.log("//////////////////////////////////////////////////////////////////////////////////////////////////////////");
console.log("//////////////////////////////////////////////////////////////////////////////////////////////////////////");
console.log("/// In header details ts config screen");
console.log(`
    @Output() showChangeTab: EventEmitter<string> = new EventEmitter<string>();
    selected${screenType}ConfigScreenType: ${screenType}ConfigScreenTabTypes;
              
    changeTab(tab: ${screenType}ConfigScreenTabTypes) {
      this.showChangeTab.emit(tab);
      this.selected${screenType}ConfigScreenType = tab;
    }
    `);

console.log("//////////////////////////////////////////////////////////////////////////////////////////////////////////");
console.log("//////////////////////////////////////////////////////////////////////////////////////////////////////////");
console.log("/// In header config screen");

console.log(`<div class="tab-container">
             
          `);
for (const element of tablesWithTitle) {
    let splittedTableStr = splitCamelCaseToString(element.table);
    const firstLetterUpperCaseElement = capitalizeFirstLetter(element.table);
    console.log(`  
            <div *ngIf="show${firstLetterUpperCaseElement}ListScreen">
              <app-${splittedTableStr}-list-screen
               [isSuperAdmin]="isSuperAdmin"
                [is${screenType}Config]="true" [companyId]="companyId" [companyName]="companyName"></app-${splittedTableStr}-list-screen>
            </div>  `)
}

console.log(`

        </div>`);



console.log("//////////////////////////////////////////////////////////////////////////////////////////////////////////");
console.log("//////////////////////////////////////////////////////////////////////////////////////////////////////////");
console.log("/// In header config ts config screen");
console.log(`
 ///// member initialization

  selected${screenType}ConfigScreenType: ${screenType}ConfigScreenTabTypes;
  `);

for (const element of tablesWithTitle) {
    const firstLetterUpperCaseElement = capitalizeFirstLetter(element.table);
    console.log(`show${firstLetterUpperCaseElement}ListScreen: boolean = false;`)
}

console.log(`
  changeTab(tab: ${screenType}ConfigScreenTabTypes) {
    this.resetTabs();
    this.selected${screenType}ConfigScreenType = tab;
  `);
for (let index = 0; index < tablesWithTitle.length; index++) {
    const element = tablesWithTitle[index];
    const titleWithLowerCase = String(element.title.toLowerCase());
    let routingPath = titleWithLowerCase.replace(" ", "-");
    routingPath = routingPath.replace(" ", "-");
    routingPath = routingPath.replace(" ", "-");
    const firstLetterUpperCaseElement = capitalizeFirstLetter(element.table);
    if (index == 0) {
        console.log(`
        if (tab == '${routingPath}') {
          this.show${firstLetterUpperCaseElement}ListScreen = true;
        }`)
    } else {
        console.log(` else if (tab == '${routingPath}') {
            this.show${firstLetterUpperCaseElement}ListScreen = true;
        }`)
    }
}



console.log(`
    }
  resetTabs() {
    `);
for (const element of tablesWithTitle) {
    const firstLetterUpperCaseElement = capitalizeFirstLetter(element.table);
    console.log(`this.show${firstLetterUpperCaseElement}ListScreen = false;`);
}
console.log(`
    }
`);


console.log(`/////////////////////////////////////////////////////`);
console.log(`In custom-model.ts`);
let tabsTypesStr = `export type ${screenType}ConfigScreenTabTypes =`;
for (let index = 0; index < tablesWithTitle.length; index++) {
    const element = tablesWithTitle[index];
    const titleWithLowerCase = String(element.title.toLowerCase());
    let routingPath = titleWithLowerCase.replace(" ", "-");
    routingPath = routingPath.replace(" ", "-");
    routingPath = routingPath.replace(" ", "-");
    if(index == 0) {
        tabsTypesStr += ` '${routingPath}' |`
    } else if((tablesWithTitle?.length - 1) == index) { 
        tabsTypesStr += ` '${routingPath}';`
    } else {
        tabsTypesStr += ` '${routingPath}' |`
    }
}
console.log(`${tabsTypesStr}`);
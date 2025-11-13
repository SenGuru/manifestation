var screenType = "MovieShootDay";
var tablesWithTitle = [
    { title: 'Summary', table: 'shootDaySummary' },
    { title: 'Movie Shoot Days', table: 'MovieShootDay' },
    { title: 'Shoot Day Budget', table: 'MovieShootDayBudget' },
    { title: 'Shoot Day Expense', table: 'MovieShootDayExpense' },
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

console.log(`
<div class="row">
    <div class="col-lg-12">
        <div class="card mt-n4 mx-n4">
            <div class="bg-warning-subtle">
                <div class="card-body pb-0 px-4">
                    <div class="row mb-3">
                        <div class="col-md">
                            <div class="row align-items-center g-3">
                                <div class="col-md-auto">
                                    <div class="col-auto">
                                        <div class="avatar-md">
                                            <img [src]="hostProfilePicture" alt="host-information-img"
                                                class="img-thumbnail rounded-circle manifest-header-img" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md">
                                    <div>
                                        <h4 class="fw-bold">Host Configuration</h4>
                                        <div class="hstack gap-3 flex-wrap">
                                            <div> <i class="ri-dashboard-fill"></i> Language : {{hostInfo?.language}}
                                            </div>
                                            <div class="vr"></div>
                                            <div><i class="ri-menu-2-line"></i> Runing Time : {{hostInfo?.runningTime |
                                                secondsToHour }}
                                            </div>
                                            <div class="vr"></div>
                                            <div><i class="ri-currency-line"></i> Budget Cost :
                                                {{hostInfo?.budgetCost}}</div>
                                            <div class="vr"></div>
                                            <div><i class="ri-team-line"></i> Company: {{hostInfo?.companyName}}</div>
                                            <div class="vr"></div>
                                            <div class="badge rounded-pill bg-info fs-12">New</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-auto">
                            <div class="hstack gap-1 flex-wrap">
                                <button type="button" class="btn py-0 fs-16 text-body material-shadow-none"
                                    (click)="backToHostsListScreen();">
                                    <i class="ri-arrow-go-back-fill"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                    
                    <div class="col-xxl-12">
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
                  </div>
                 <!-- end card body -->
                </div>
            </div>
            <!-- end card -->
        </div>
        <!-- end col -->
     </div>`);
 

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
let tabsTypesStr = `export type ${screenType}ConfigScreenTabTypes:`;
for (let index = 0; index < tablesWithTitle.length; index++) {
    const element = tablesWithTitle[index];
    const titleWithLowerCase = String(element.title.toLowerCase());
    let routingPath = titleWithLowerCase.replace(" ", "-");
    routingPath = routingPath.replace(" ", "-");
    routingPath = routingPath.replace(" ", "-");
    if (index == 0) {
        tabsTypesStr += ` ${routingPath} |`
    } else if ((tablesWithTitle?.length - 1) == index) {
        tabsTypesStr += ` ${routingPath};`
    } else {
        tabsTypesStr += ` ${routingPath} |`
    }
}
console.log(`${tabsTypesStr}`);
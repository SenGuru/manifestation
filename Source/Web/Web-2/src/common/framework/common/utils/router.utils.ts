import { MenuItem } from "src/common/models/app-custom-model";

export class RouterUtils {
    
 public static updateUrlWithId(menuData: MenuItem[], IdText: string, replacementId: string): MenuItem[] {
    const result: MenuItem[] = menuData;
    result.forEach((menuItem: MenuItem) => {
      menuItem.link = menuItem.link?.replace(IdText, replacementId);
      if (menuItem.subItems) {
        menuItem.subItems.forEach((subItem: MenuItem) => {
          subItem.link = subItem.link?.replace(IdText, replacementId);

          if (subItem.subItems) {
            subItem.subItems.forEach((child1Item: MenuItem) => {
              child1Item.link = child1Item.link?.replace(IdText, replacementId);

              if (child1Item.subItems) {
                child1Item.subItems.forEach((child2Item: MenuItem) => {
                  child2Item.link = child2Item.link?.replace(IdText, replacementId);

                })
              }
            })
          }
        })
      }
    });
    return result;
  }

}
import {addSubRegion} from "./subRegion";
import {addCriteria} from "./criteria";

const subRegionBtn = document.getElementById("sub-region-btn") as HTMLAnchorElement;
subRegionBtn.addEventListener("click", addSubRegion);

const addCriteriaBtn = document.getElementById("criteria-btn") as HTMLAnchorElement;
addCriteriaBtn.addEventListener("click",addCriteria);


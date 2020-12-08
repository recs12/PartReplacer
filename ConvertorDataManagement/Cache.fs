//module Cache

//    type ProperySets() =
//        interface SolidEdgeFileProperties.PropertySets with
//            member this.Application = raise (System.NotImplementedException())
//            member this.Close() = raise (System.NotImplementedException())
//            member this.Count = raise (System.NotImplementedException())
//            member this.CreateCustonPropertySet() = raise (System.NotImplementedException())
//            member this.GetEnumerator() = raise (System.NotImplementedException())
//            member this.GetFamilyOfAssemblyMemberNames(fileName, memberCount, memberNames) = raise (System.NotImplementedException())
//            member this.IsFileFamilyOfAssembly(fileName, bFamilyOfAssembly) = raise (System.NotImplementedException())
//            member this.IsFileWeldmentAssembly(fileName, bWeldmentAssembly) = raise (System.NotImplementedException())
//            member this.Item
//                with get (index) = raise (System.NotImplementedException())
//            member this.Parent = raise (System.NotImplementedException())
//            member this.Save() = raise (System.NotImplementedException())
//            member this.Open(a,b) = raise (System.NotImplementedException())


//    type Properties() =
//        interface SolidEdgeFileProperties.Properties with
//            member this.Add(name, value) = raise (System.NotImplementedException())
//            member this.Application = raise (System.NotImplementedException())
//            member this.Count = raise (System.NotImplementedException())
//            member this.GetEnumerator() = raise (System.NotImplementedException())
//            member this.Name = raise (System.NotImplementedException())
//            member this.Parent = raise (System.NotImplementedException())
//            member this.PropertyByID
//                with get (propID) = raise (System.NotImplementedException())
//            member this.Save() = raise (System.NotImplementedException())
//            member this.Item
//                with get (index) = raise (System.NotImplementedException())


//    type Property() =
//        interface SolidEdgeFileProperties.Property with
//            member this.Item
//                with get (index) = raise (System.NotImplementedException())


//    let ExtractJdeFromCad location =

//        let prop = new ProperySets()
//        let prop = prop :> SolidEdgeFileProperties.PropertySets
//        prop.Open(location, true)

//        let pr = new Properties()
//        let pr = pr :> SolidEdgeFileProperties.Properties
//        let prr = pr.["Custom"]
//        prr = po :>po
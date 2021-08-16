# FirebaseAuthRepro
This is an example repro for the issue https://github.com/firebase/quickstart-unity/issues/1115. Which outlines that when on iOS attempting to do any read from storage that requires the auth token to not be null will fail with an `'User' is not authorized to perform the desired action` exception regardless of their authentication state.

## Setup Required
- A firebase project which has Auth with Anonymous Login Enabled, Firestore and Storage set up
- Firestore with any kind of security rules which check for the user auth state. [Our settings.](https://user-images.githubusercontent.com/28091817/129323078-5f2d575d-8da9-475d-9f1b-c70bc4043307.png)
- Storage with any kind of security rules which check for the user auth state. [Our settings.](https://user-images.githubusercontent.com/28091817/129323130-55526d0a-ced4-4f46-b9e2-292940f4a116.png)
- Import [Auth, Storage, Firestore] 8.1.0 SDKs
- Import your `google-services.json` + `google-services.plist` into the assets folder.
- Open up `Scenes/Repro`
- Select the gameobject `ReproCanvas/FirestoreArea` and setup what data you want to read + write *(Document Fields do not matter)*. [Our Inspector](https://user-images.githubusercontent.com/28091817/129325383-eac8a1be-84de-49a6-b5cd-d21d6b002c3b.png) + [Our Database](https://user-images.githubusercontent.com/28091817/129325486-551e9cb4-7579-4e2c-bf64-f107248121c3.png)
- Select the gameobject `ReproCanvas/StorageArea` and setup the data you want to read + write *(Content of the files does not matter)*. [Our Inspector](https://user-images.githubusercontent.com/28091817/129325675-9a2b5bf7-a3e2-4f58-b54d-ca7c129ac5e3.png) + [Our Database](https://user-images.githubusercontent.com/28091817/129325750-f63919ec-2d20-4d8d-885f-a9c5e8c23fdc.png)
- Finally make and deploy an iOS build, and run the app.
- Notice that only [Storage read fails](https://user-images.githubusercontent.com/28091817/129323130-55526d0a-ced4-4f46-b9e2-292940f4a116.png), even though everything else succeeds and the database should allow for the data. 

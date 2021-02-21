# GraphQL HotChocolate Workshop
This is a short note version of the HotChocolate Workshop found [here](https://github.com/ChilliCream/graphql-workshop). I also created a [repository](https://github.com/tygore587/graphql-workshop-project), where you can find the complete workshop project.

## General Setup ([Reference](https://github.com/ChilliCream/graphql-workshop/blob/master/docs/1-creating-a-graphql-server-project.md#create-a-new-graphql-server-project))

Just follow the steps to setup the project

## Database objects
```csharp
namespace ConferencePlanner.GraphQL.Data
 {
     public class Speaker
     {
         public int Id { get; set; }

         [Required]
         [StringLength(200)]
         public string Name { get; set; }

         [StringLength(4000)]
         public string Bio { get; set; }

         [StringLength(1000)]
         public virtual string WebSite { get; set; }
     }
 }
```

## Mutations

- add new data to the database
- consists of 3 components
	- input
		- to create new object
	- payload
		- return object after creation
	- mutation itself
		- named as verbs

### Example
3 components
- input: `AddSpeakerInput`
- payload: `AddSpeakerPayload`
- mutation: `addSpeaker`

#### Classes
1. Input
```csharp
namespace ConferencePlanner.GraphQL
 {
     public record AddSpeakerInput(
         string Name,
         string Bio,
         string WebSite);
 }
```
2. Payload
```csharp
using ConferencePlanner.GraphQL.Data;

 namespace ConferencePlanner.GraphQL
 {
     public class AddSpeakerPayload
     {
         public AddSpeakerPayload(Speaker speaker)
         {
             Speaker = speaker;
         }

         public Speaker Speaker { get; }
     }
 }
```
3. Mutation
```csharp
using System.Threading.Tasks;
 using ConferencePlanner.GraphQL.Data;
 using HotChocolate;

 namespace ConferencePlanner.GraphQL
 {
     public class Mutation
     {
         public async Task<AddSpeakerPayload> AddSpeakerAsync(
             AddSpeakerInput input,
             [Service] ApplicationDbContext context)
         {
             var speaker = new Speaker
             {
                 Name = input.Name,
                 Bio = input.Bio,
                 WebSite = input.WebSite
             };

             context.Speakers.Add(speaker);
             await context.SaveChangesAsync();

             return new AddSpeakerPayload(speaker);
         }
     }
 }
```

### Using mutation
#### Structure
```graphql
# name of the mutation method (can be completly defined free)
mutation <name of mutation method> {
	# mutation name defined in code and can be found also in graphql explorer
	<mutationName>(input: {
		# Here is the object we defined as <mutationName>Input (Example: AddSpeakerInput)
	}) {
		# Here we add which fields we want to get from <mutationName>Payload (Example: AddSpeakerPayload)
	}
}
```
#### Example (from Mutation exmaple above)
```graphql
mutation AddSpeaker {
	addSpeaker(input: {
	 name: "Speaker Name"
	 bio: "Speaker Bio"
	 webSite: "http://speaker.website" 
	}) {
	 speaker {
	   id
	 }
	}
}
```
Here we want to create a new speaker with following fields and we want to get the id of the speaker after the speaker was created.

## Queries
To query data we use the keyword query.

### Structure
```text
query <name of query> {
	<name of objects to get> {
		<fields to get from object>
	}
}
```
#### Example
```graphql
 query GetSpeakerNames {
   speakers {
     name
   }
 }
```
Here we get the names of all speakers.

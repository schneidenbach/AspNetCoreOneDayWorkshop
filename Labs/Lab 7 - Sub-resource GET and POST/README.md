# Sub-resource GET and POST

The goal of this lab is to allow you to practice implementing and testing a REST API that uses resources that are owned by other resources (in this case, it's a resource owned by Job called JobPhase).

## Concepts

- Implement a create sub-resource GET request using the CQRS pattern.
- Implement a create sub-resource POST request using the CQRS pattern.

## Tasks

Unlike the other labs, much of the code is there for you already - your job instead is to a) fill in the Job Phase handlers/requests and b) write the tests. **If you get stuck, you can always peek at the code in the Completed solution!** 

You will need to fill in the following files:

* JobPhases/CreateJobPhaseForJob/CreateJobPhaseForJobHandler
* JobPhases/CreateJobPhaseForJob/CreateJobPhaseForJobRequestValidator
* JobPhases/GetJobPhasesForJob/GetJobPhasesForJobHandler
* Tests/CreateJobPhaseForJobTests
* GetJobPhasesTests

Use all of the other tests available to copy over techniques, including making POST requests and deserializing invalid requests to a class where errors can be read from. (You can get a lot of this from the CreateJobTests file - you mainly need to refactor to use JobPhases instead.)
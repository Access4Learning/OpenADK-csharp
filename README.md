OpenADK (C# version)
====================

Instructions for generating data model files
--------------------------------------------

Ant must be installed on build computer.

1. Download both OpenADK .NET and OpenADK Java as sibling projects from LaunchPad.
2. From the command line, browse to the open-adk-java/adk-generator folder.
3. Run 'ant dotnet.deploy'

This will build the .NET SDO libraries and add them to the correct directories in the open-adk-csharp project. Once this is run, open your IDE. 

In the case of Visual Studio, the .sln files have already been created so you can open the solution of your choice. They are located in the 'src' directory.

In Visual Studio, use the solution explorer to add the newly generated SDO directory.  
Select the project, i.e. OpenADK-UK-SDO. Then at the top of the solution explorer select the 'show all files' button. The generated folders should appear. Right click on the new folders and select 'add to project'.

The solution should now be ready to go.

Contributing
------------

For some background information regarding the branching model used by this project, refer to the article [A successful Git branching model][1]. The ideas from that article have been combined with the collaborative development techniques recommended by GitHub (specifically [Using Pull Requests][2]) to form these procedures.

### Pre-requisities ###

* [Git][3] installed.
* A strong understanding of Git. For an introduction, read [Getting Started - Git Basics][4].
* A GitHub account and Pull access to the [OpenADK][5] repositories.

### Defined Repositories ###

The procedures following refer to 3 Git repositories:

* The remote [OpenADK-csharp][8] repository containing a _central_ copy of the code.
* Your remote repository created within your GitHub account.
* Your local repository residing on your local machine.

### Getting the code ###

1. Fork the [OpenADK-csharp][8] repository into your remote repository (click on the "Fork" button).
2. Clone your fork to create your local repository. This will create a _OpenADK-csharp_ directory on your local machine.

    ```dos
    c:\dev> git clone https://github.com/USERNAME/OpenADK-csharp.git
    ```

3. Add the _upstream_ ([OpenADK-csharp][8]) repository as a remote. This will allow you to keep track of the [OpenADK-csharp][8] repository and pull in updates.

    ```dos
    c:\dev> cd OpenADK-csharp
    c:\dev\OpenADK-csharp> git remote add upstream git://github.com/open-adk/OpenADK-csharp.git
    ```

4. Create a _topic_ branch in your remote repository based upon the _develop_ branch and switch to it (your local repository). __Do not base your _topic_ branch on any other branch!__

    ```dos
    c:\dev\OpenADK-csharp> git checkout -t origin/develop
    c:\dev\OpenADK-csharp> git branch ISSUE_XXX develop
    c:\dev\OpenADK-csharp> git checkout ISSUE_XXX
    ```

### Submitting a change ###

5. Ensure that all changes applied to the _topic_ branch (locally) have been committed to your remote repository (with an appropriate message). The _-a_ flag skips _staging_.

    ```dos
    c:\dev\OpenADK-csharp> git commit -a -m MESSAGE
    ```

6. Before submission, ensure that your remote repository is up-to-date.

    ```dos
    c:\dev\OpenADK-csharp> git checkout develop
    c:\dev\OpenADK-csharp> git fetch upstream
    c:\dev\OpenADK-csharp> git merge upstream/develop
    ```

7. Rebase the changes in your local repository (making for a cleaner history).

    ```dos
    c:\dev\OpenADK-csharp> git checkout ISSUE_XXX
    c:\dev\OpenADK-csharp> git rebase develop
    ```

8. Resolve and recommit [merge conflicts][7] (if any), and continue the rebase.

    ```dos
    c:\dev\OpenADK-csharp> git rebase --continue
    ```

9. Push your local changes to your remote repository.

    ```dos
    c:\dev\OpenADK-csharp> git push origin ISSUE_XXX
    ```

### Issue a Pull Request ###

The [Using Pull Requests][2] article provides a comprehensive guide for issuing a Pull Request.

10. Browse to your remote (forked) repository on the GitHub site.

11. Swith to the _ISSUE_XXX_ branch and press the "Pull Request" button.

12. Review the Pull Request details and provide a meaningful title (with issue number if appropriate) and description of your change. It is important to ensure that the _base branch_ is set to _develop_ and the _head branch_ is set to _ISSUE_XXX_.

13. Press the "Send pull request" button.

Project Maintenance
-------------------

### Merge a Pull Request ###

GitHub provides the ability for a Pull Request to be automatically merged. If you have the appropriate permissions, the "Merge pull request" button can be used from the "Pull Requests" page. Alternatively, you could perform a manual merge on the command line.

1. Check out a new branch of the OpenADK-csharp repository to test the changes in.

    ```dos
    c:\dev\OpenADK-csharp> git checkout -b USERNAME-ISSUE_XXX develop
    ```

2. Bring in the changes from the requesting repository into your new branch and perform your tests.

    ```dos
    c:\dev\OpenADK-csharp> git pull https://github.com/USERNAME/OpenADK-csharp.git ISSUE_XXX
    ```

3. Merge the changes and update the OpenADK-csharp repository.

    ```dos
    c:\dev\OpenADK-csharp> git checkout develop
    c:\dev\OpenADK-csharp> git merge USERNAME-ISSUE_XXX
    c:\dev\OpenADK-csharp> git push origin develop
    ```

[1]: http://nvie.com/posts/a-successful-git-branching-model
[2]: https://help.github.com/articles/using-pull-requests
[3]: http://git-scm.com/downloads
[4]: http://git-scm.com/book/en/Getting-Started-Git-Basics
[5]: https://github.com/organizations/open-adk
[6]: forkButton.png "Fork"
[7]: http://git-scm.com/book/en/Git-Branching-Basic-Branching-and-Merging#Basic-Merge-Conflicts
[8]: https://github.com/open-adk/OpenADK-csharp
[9]: http://symfony.com/doc/current/contributing/code/patches.html

import { Routes } from '@angular/router';
import {TaskListComponent} from './components/task-list/task-list.component';
import {UpdateTaskComponent} from './components/update-task/update-task.component';
import {AddTaskComponent} from './components/add-task/add-task.component';

export const routes: Routes = [
  { path: '', component: TaskListComponent },
  { path: 'add-task', component: AddTaskComponent },
  { path: 'update-task/:id', component: UpdateTaskComponent },
];


import {Component, Signal} from '@angular/core';
import { Task } from '../../models/task';
import {TaskService} from '../../services/task.service';
import {
  MatCell, MatCellDef,
  MatColumnDef,
  MatHeaderCell, MatHeaderCellDef, MatHeaderRow, MatHeaderRowDef,
  MatRow, MatRowDef,
  MatTable
} from '@angular/material/table';
import {MatButton, MatFabButton} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';
import {RouterLink} from '@angular/router';
import {TaskStatus} from '../../models/task-status';

@Component({
  selector: 'app-task-list',
  imports: [
    MatTable,
    MatHeaderCell,
    MatColumnDef,
    MatCell,
    MatButton,
    MatRow,
    MatHeaderRow,
    MatIcon,
    RouterLink,
    MatFabButton,
    MatHeaderCellDef,
    MatCellDef,
    MatHeaderRowDef,
    MatRowDef
  ],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css',
  standalone: true
})
export class TaskListComponent {
  taskStatus = TaskStatus;
  displayedColumns: string[] = ['id', 'name', 'description', 'status', 'assignedTo', 'actions'];

  $tasks: Signal<Task[]>;

  constructor(private taskService: TaskService) {
    this.$tasks = this.taskService.getTasks();
  }
}

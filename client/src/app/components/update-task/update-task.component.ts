import {Component, OnInit} from '@angular/core';
import {TaskStatus} from '../../models/task-status';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {FormBuilder, FormGroup, ReactiveFormsModule} from '@angular/forms';
import {MatOption} from '@angular/material/core';
import {MatSelect} from '@angular/material/select';
import {NgForOf} from '@angular/common';
import {MatButton} from '@angular/material/button';
import {TaskService} from '../../services/task.service';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-update-task',
  imports: [
    ReactiveFormsModule,
    MatOption,
    MatSelect,
    NgForOf,
    MatButton,
    MatLabel,
    MatFormField
  ],
  templateUrl: './update-task.component.html',
  styleUrl: './update-task.component.css'
})
export class UpdateTaskComponent implements OnInit {
  updateForm!: FormGroup;
  taskId!: number;
  statuses = Object.keys(TaskStatus)
    .filter(key => isNaN(Number(key)))
    .map(key => ({
      label: key,
      value: TaskStatus[key as keyof typeof TaskStatus]
    }));

  constructor(
    private route: ActivatedRoute,
    private taskService: TaskService,
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.taskId = Number(this.route.snapshot.paramMap.get('id'));
    this.updateForm = this.fb.group({
      status: [TaskStatus.NotStarted]
    });
  }

  onSubmit(): void {
    const newStatus = this.updateForm.get('status')?.value;
    this.taskService.updateTaskStatus(this.taskId, newStatus).subscribe((_ => this.router.navigate(['/'])));
  }
}
